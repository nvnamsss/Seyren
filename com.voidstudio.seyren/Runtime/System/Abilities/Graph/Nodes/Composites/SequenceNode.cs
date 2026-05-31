namespace Seyren.System.Abilities
{
    /// <summary>
    /// Ticks children in order. Advances to the next child when the current one
    /// returns <see cref="NodeState.Success"/>. Returns <see cref="NodeState.Failure"/>
    /// immediately if any child fails. Returns <see cref="NodeState.Success"/> once
    /// all children have succeeded.
    /// </summary>
    public class SequenceNode : IAbilityNode, INodeContainer
    {
        private readonly IAbilityNode[] _children;
        private int _currentIndex;

        public SequenceNode(params IAbilityNode[] children)
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
            return new SequenceNode(updated);
        }

        public NodeState Tick(AbilityGraphContext context, float deltaTime)
        {
            if (_currentIndex >= _children.Length)
                return NodeState.Success;

            NodeState result = _children[_currentIndex].Tick(context, deltaTime);

            if (result == NodeState.Failure)
                return NodeState.Failure;

            if (result == NodeState.Success)
            {
                _currentIndex++;
                if (_currentIndex >= _children.Length)
                    return NodeState.Success;
            }

            return NodeState.Running;
        }
    }
}
