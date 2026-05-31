using System;

namespace Seyren.System.Abilities
{
    /// <summary>
    /// Graph modifier that wraps the ability's entry node so that an explosion
    /// fires on whichever comes first: a hit event (<c>"onHit"</c>) or the
    /// ability expiring (<c>"cancel"</c>).
    ///
    /// The resulting graph root is:
    /// <code>
    /// ParallelNode(All,
    ///     originalEntryNode,
    ///     SelectorNode(
    ///         OnHitEventNode(explosionNode),
    ///         OnExpireEventNode(explosionNode)
    ///     )
    /// )
    /// </code>
    /// Both branches of the <see cref="SelectorNode"/> hold independent explosion
    /// node instances produced by <see cref="ExplosionNodeFactory"/>. The selector
    /// ensures exactly one of them succeeds (hit wins, then expire never executes).
    /// </summary>
    public class ExplosionModifier : IAbilityGraphModifier, IGraphAbilityVFXProvider
    {
        /// <summary>
        /// Factory invoked once per event branch to produce a fresh
        /// <see cref="IAbilityNode"/> that performs the explosion (e.g. a
        /// <see cref="DealDamageNode"/> or <see cref="SpawnEntityNode"/>).
        /// Called during <see cref="Apply"/>; the returned nodes run at Tick time.
        /// </summary>
        public Func<IAbilityNode> ExplosionNodeFactory { get; }

        public ExplosionModifier(Func<IAbilityNode> explosionNodeFactory)
        {
            ExplosionNodeFactory = explosionNodeFactory;
        }

        public AbilityGraph Apply(AbilityGraph graph, AbilityContext context)
        {
            // Use ParallelNode(Any) so BOTH event nodes are ticked every frame.
            // SelectorNode would be wrong here: it only advances to the next child when the
            // current returns Failure, but OnHitEventNode returns Running indefinitely while
            // waiting — so OnExpireEventNode would never be reached.
            IAbilityNode triggerNode = new ParallelNode(CompletionPolicy.Any,
                new OnHitEventNode(ExplosionNodeFactory()),
                new OnExpireEventNode(ExplosionNodeFactory())
            );

            IAbilityNode root = new ParallelNode(CompletionPolicy.All,
                graph.EntryNode,
                triggerNode
            );

            return new AbilityGraph(root);
        }

        /// <inheritdoc/>
        public IGraphAbilityVFX GetVFX() => new ExplosionDefaultVFX();
    }
}
