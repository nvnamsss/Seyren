namespace Seyren.System.Abilities
{
    /// <summary>
    /// Controls when a <see cref="ParallelNode"/> reports completion.
    /// </summary>
    public enum CompletionPolicy
    {
        /// <summary>All children must succeed before the node returns Success.</summary>
        All,
        /// <summary>The first child to succeed causes the node to return Success immediately.</summary>
        Any
    }

    /// <summary>
    /// Ticks all children every frame. Completion is determined by
    /// <see cref="CompletionPolicy"/>:
    /// <list type="bullet">
    ///   <item><see cref="CompletionPolicy.All"/> — returns Success when all children succeed;
    ///         returns Failure as soon as any child fails.</item>
    ///   <item><see cref="CompletionPolicy.Any"/> — returns Success when any child succeeds;
    ///         returns Failure when all children have failed.</item>
    /// </list>
    /// </summary>
    public class ParallelNode : IAbilityNode, INodeContainer
    {
        private readonly IAbilityNode[] _children;
        private readonly bool[] _completed;
        private readonly CompletionPolicy _policy;

        // Exposed for graph-level cloning (e.g. MultishotModifier).
        internal CompletionPolicy Policy => _policy;

        public ParallelNode(CompletionPolicy policy, params IAbilityNode[] children)
        {
            _policy = policy;
            _children = children;
            _completed = new bool[children.Length];
        }

        public IAbilityNode[] GetChildren() => _children;

        public IAbilityNode WithChild(IAbilityNode oldChild, IAbilityNode newChild)
        {
            var updated = (IAbilityNode[])_children.Clone();
            for (int i = 0; i < updated.Length; i++)
                if (updated[i] == oldChild) { updated[i] = newChild; break; }
            return new ParallelNode(_policy, updated);
        }

        public NodeState Tick(AbilityGraphContext context, float deltaTime)
        {
            int completedCount = 0;
            int failedCount = 0;

            for (int i = 0; i < _children.Length; i++)
            {
                if (_completed[i])
                {
                    completedCount++;
                    continue;
                }

                NodeState result = _children[i].Tick(context, deltaTime);

                if (result == NodeState.Success)
                {
                    _completed[i] = true;
                    completedCount++;

                    if (_policy == CompletionPolicy.Any)
                        return NodeState.Success;
                }
                else if (result == NodeState.Failure)
                {
                    _completed[i] = true;
                    completedCount++;
                    failedCount++;

                    if (_policy == CompletionPolicy.All)
                        return NodeState.Failure;
                }
            }

            if (completedCount < _children.Length)
                return NodeState.Running;

            // All slots accounted for
            return failedCount == _children.Length ? NodeState.Failure : NodeState.Success;
        }
    }
}
