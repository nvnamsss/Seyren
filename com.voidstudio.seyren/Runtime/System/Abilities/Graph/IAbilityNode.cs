namespace Seyren.System.Abilities
{
    /// <summary>
    /// A single executable unit in an ability graph.
    /// Nodes are ticked once per server frame by their parent graph instance.
    /// </summary>
    public interface IAbilityNode
    {
        /// <summary>
        /// Advances the node by one tick.
        /// </summary>
        /// <param name="context">Shared runtime data for the current ability instance.</param>
        /// <param name="deltaTime">Seconds elapsed since the last tick.</param>
        /// <returns>
        /// <see cref="NodeState.Running"/> if the node needs another tick,
        /// <see cref="NodeState.Success"/> if it completed successfully,
        /// <see cref="NodeState.Failure"/> if it failed.
        /// </returns>
        NodeState Tick(AbilityGraphContext context, float deltaTime);
    }
}
