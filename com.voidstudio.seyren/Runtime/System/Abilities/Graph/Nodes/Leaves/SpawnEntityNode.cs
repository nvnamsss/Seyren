using System;
using Seyren.Projectiles;

namespace Seyren.System.Abilities
{
    /// <summary>
    /// Invokes a factory to spawn an entity on the first tick.
    /// The result is added to <see cref="AbilityGraphContext.SpawnedEntities"/> and,
    /// when <see cref="EntityKey"/> is provided, stored in
    /// <see cref="AbilityGraphContext.Variables"/> for downstream nodes to retrieve.
    /// </summary>
    public class SpawnEntityNode : IAbilityNode
    {
        private readonly Func<AbilityGraphContext, object> _spawnFactory;
        private readonly string _entityKey;
        private bool _fired;

        // Exposed so graph modifiers (e.g. MultishotModifier) can clone this node.
        internal Func<AbilityGraphContext, object> SpawnFactory => _spawnFactory;
        internal string EntityKey => _entityKey;

        /// <param name="spawnFactory">Called once to produce the spawned entity.</param>
        /// <param name="entityKey">Optional key under which the entity is stored in context variables.</param>
        public SpawnEntityNode(Func<AbilityGraphContext, object> spawnFactory, string entityKey = null)
        {
            _spawnFactory = spawnFactory;
            _entityKey = entityKey;
        }

        public NodeState Tick(AbilityGraphContext context, float deltaTime)
        {
            if (_fired) return NodeState.Success;

            object entity = _spawnFactory(context);
            context.SpawnedEntities.Add(entity);
            if (_entityKey != null)
                context.Variables[_entityKey] = entity;

            // Notify VFX chain that an entity was spawned.
            if (_entityKey != null)
                context.OnEntitySpawnedCallback?.Invoke(_entityKey, entity);

            // Wire OnEntityCompletedCallback for projectile entities so VFX
            // can react when the projectile hits or expires.
            if (entity is IProjectile proj && _entityKey != null)
                proj.OnCompleted += _ => context.OnEntityCompletedCallback?.Invoke(_entityKey, entity);

            _fired = true;
            return NodeState.Success;
        }
    }
}
