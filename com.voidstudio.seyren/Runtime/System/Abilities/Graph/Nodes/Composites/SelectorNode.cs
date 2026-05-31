namespace Seyren.System.Abilities
{
    /// <summary>
    /// Ticks children in order. Returns <see cref="NodeState.Success"/> as soon
    /// as any child succeeds. Advances to the next child when the current one
    /// returns <see cref="NodeState.Failure"/>. Returns
    /// <see cref="NodeState.Failure"/> only when every child has failed.
    /// </summary>
    public class SelectorNode : IAbilityNode, INodeContainer
    {
        private readonly IAbilityNode[] _children;
        private int _currentIndex;

        public SelectorNode(params IAbilityNode[] children)
        {
            _children = children;
            _currentIndex = 0;
        }

        public IAbilityNode[] GetChildren() => _children;

        public IAbilityNode WithChild(IAbilityNode oldChild, IAbilityNode newChild)
        {
            var updated = (IAbilityNode[])_children.Clone();
            for (int i = 0; i < updated.Length; i++)
                if (updated[i] == oldChild) { updated[i] = newChild; break; }
            return new SelectorNode(updated);
        }

        public NodeState Tick(AbilityGraphContext context, float deltaTime)
        {
            if (_currentIndex >= _children.Length)
                return NodeState.Failure;

            NodeState result = _children[_currentIndex].Tick(context, deltaTime);

            if (result == NodeState.Success)
                return NodeState.Success;

            if (result == NodeState.Failure)
            {
                _currentIndex++;
                if (_currentIndex >= _children.Length)
                    return NodeState.Failure;
            }

            return NodeState.Running;
        }
    }
}
