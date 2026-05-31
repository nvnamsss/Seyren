using Seyren.Projectiles;
using UnityEngine;

namespace Seyren.System.Abilities
{
    /// <summary>
    /// Graph modifier that duplicates the entire ability graph N times and runs
    /// each copy in parallel, fanning the extra shots around the cast direction.
    ///
    /// Each branch runs in its own context scope (forked from the parent context)
    /// so that per-branch variables (e.g. "orb") are fully isolated.  This means
    /// abilities that have downstream nodes reading spawned-entity keys — such as
    /// FrozenOrb's shard-loop reading "orb" — work correctly for every shot.
    ///
    /// The first branch (index 0) keeps the original cast direction.
    /// Extra branches alternate ±SpreadAngleDeg offsets (+15°, -15°, +30°, …).
    ///
    /// Usage: ability.AddModifier(new MultishotModifier(3));
    /// When Count == 0 the modifier falls back to context.level.
    /// </summary>
    public class MultishotModifier : IAbilityGraphModifier
    {
        /// <summary>Number of shots to fire. When 0, uses context.level instead.</summary>
        public int Count { get; }

        /// <summary>Angular gap in degrees between adjacent shots in the fan.</summary>
        public float SpreadAngleDeg { get; }

        public MultishotModifier(int count = 0, float spreadAngleDeg = 15f)
        {
            Count = count;
            SpreadAngleDeg = spreadAngleDeg;
        }

        public AbilityGraph Apply(AbilityGraph graph, AbilityContext context)
        {
            int shotCount = Count > 0 ? Count : context.level;
            if (shotCount <= 1) return graph;

            Vector3 baseDir = ComputeDirection(context);
            var branches = new IAbilityNode[shotCount];

            for (int i = 0; i < shotCount; i++)
            {
                // Fan spread: 0 → 0°, 1 → +spread, 2 → -spread, 3 → +2*spread, …
                float sign  = (i % 2 == 1) ? 1f : -1f;
                float angle = i == 0 ? 0f : sign * Mathf.Ceil(i / 2f) * SpreadAngleDeg;
                Vector3 dir = Quaternion.Euler(0f, angle, 0f) * baseDir;

                // Clone produces a fresh node tree with independent mutable state.
                IAbilityNode clonedRoot = CloneNode(graph.EntryNode);

                // For branches that need a direction override, wrap the first
                // SpawnEntityNode so the projectile launches in the rotated direction.
                if (i != 0)
                    clonedRoot = ApplyDirectionOverride(clonedRoot, dir);

                // IsolatedBranchNode forks the context on first tick so this branch
                // has its own Variables dictionary — prevents key collisions between
                // parallel orbs (e.g. each branch writes "orb" into its own scope).
                branches[i] = new IsolatedBranchNode(clonedRoot);
            }

            return new AbilityGraph(new ParallelNode(CompletionPolicy.All, branches));
        }

        // ── Cloning ──────────────────────────────────────────────────────────

        /// <summary>
        /// Recursively clones a node tree so each parallel branch has independent
        /// mutable state (counters, timers, fired flags, etc.).
        /// </summary>
        private static IAbilityNode CloneNode(IAbilityNode node)
        {
            if (node is SpawnEntityNode spawn)
                return new SpawnEntityNode(spawn.SpawnFactory, spawn.EntityKey);

            if (node is SequenceNode)
            {
                IAbilityNode[] children = ((INodeContainer)node).GetChildren();
                var cloned = new IAbilityNode[children.Length];
                for (int i = 0; i < children.Length; i++)
                    cloned[i] = CloneNode(children[i]);
                return new SequenceNode(cloned);
            }

            if (node is ParallelNode parallel)
            {
                IAbilityNode[] children = parallel.GetChildren();
                var cloned = new IAbilityNode[children.Length];
                for (int i = 0; i < children.Length; i++)
                    cloned[i] = CloneNode(children[i]);
                return new ParallelNode(parallel.Policy, cloned);
            }

            if (node is LoopNode loop)
                return new LoopNode(loop.Mode, loop.ChildFactory, loop.MaxCount, loop.Duration, loop.Interval);

            // Unknown node type: return as-is (stateless nodes, custom leaves, etc.)
            return node;
        }

        // ── Direction override ────────────────────────────────────────────────

        /// <summary>
        /// Walks the cloned tree and wraps the first <see cref="SpawnEntityNode"/>
        /// so the spawned projectile is launched in <paramref name="direction"/>.
        /// </summary>
        private static IAbilityNode ApplyDirectionOverride(IAbilityNode node, Vector3 direction)
        {
            if (node is SpawnEntityNode spawn)
            {
                var originalFactory = spawn.SpawnFactory;
                return new SpawnEntityNode(ctx =>
                {
                    object entity = originalFactory(ctx);
                    if (entity is CommonProjectile proj)
                    {
                        proj.direction = direction;
                        if (proj.gameObject != null)
                            proj.gameObject.transform.rotation = Quaternion.LookRotation(direction);
                    }
                    return entity;
                }, spawn.EntityKey);
            }

            if (node is INodeContainer container)
            {
                IAbilityNode[] children = container.GetChildren();
                for (int i = 0; i < children.Length; i++)
                {
                    IAbilityNode replacement = ApplyDirectionOverride(children[i], direction);
                    if (replacement != children[i])
                        return container.WithChild(children[i], replacement);
                }
            }

            return node;
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private static Vector3 ComputeDirection(AbilityContext context)
        {
            if (context.end.HasValue && context.start.HasValue)
            {
                Vector3 d = context.end.Value - context.start.Value;
                d.y = 0f;
                if (d.sqrMagnitude > 0.0001f) return d.normalized;
            }

            if (context.location.HasValue)
            {
                Vector3 d = context.location.Value - context.caster.Location;
                d.y = 0f;
                if (d.sqrMagnitude > 0.0001f) return d.normalized;
            }

            return Vector3.forward;
        }

        // ── Private node: isolated context per branch ─────────────────────────

        /// <summary>
        /// Wraps an <see cref="IAbilityNode"/> and ticks it with a forked context
        /// so the branch has its own <see cref="AbilityGraphContext.Variables"/>
        /// namespace, isolated from all sibling branches.
        /// </summary>
        private sealed class IsolatedBranchNode : IAbilityNode
        {
            private readonly IAbilityNode _root;
            private AbilityGraphContext _branchContext;

            public IsolatedBranchNode(IAbilityNode root) { _root = root; }

            public NodeState Tick(AbilityGraphContext context, float deltaTime)
            {
                // Fork once on first tick; reuse the same fork for every subsequent tick.
                if (_branchContext == null)
                    _branchContext = context.Fork();

                NodeState result = _root.Tick(_branchContext, deltaTime);

                // When this branch finishes, bubble "cancel" to the parent context so
                // that outer modifiers (e.g. ExplosionModifier's OnExpireEventNode)
                // which listen on the parent context can react to branch completion.
                if (result != NodeState.Running)
                    context.FireEvent("cancel");

                return result;
            }
        }
    }
}

