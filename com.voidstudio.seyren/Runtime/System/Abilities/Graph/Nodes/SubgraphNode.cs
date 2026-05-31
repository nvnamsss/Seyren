namespace Seyren.System.Abilities
{
    /// <summary>
    /// Embeds a nested <see cref="AbilityGraph"/> as a single node.
    /// Creates an <see cref="AbilityGraphInstance"/> on the first tick using the
    /// current context and drives it to completion, then returns Success.
    /// </summary>
    public class SubgraphNode : IAbilityNode
    {
        private readonly AbilityGraph _childGraph;
        private AbilityGraphInstance _childInstance;

        public SubgraphNode(AbilityGraph childGraph)
        {
            _childGraph = childGraph;
        }

        public NodeState Tick(AbilityGraphContext context, float deltaTime)
        {
            if (_childInstance == null)
                _childInstance = new AbilityGraphInstance(_childGraph, context);

            _childInstance.Tick(deltaTime);

            return _childInstance.IsActive ? NodeState.Running : NodeState.Success;
        }
    }
}
