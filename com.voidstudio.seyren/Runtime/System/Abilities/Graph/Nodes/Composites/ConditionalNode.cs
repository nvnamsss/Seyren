using System;

namespace Seyren.System.Abilities
{
    /// <summary>
    /// Evaluates a predicate once on the first tick and delegates all subsequent
    /// ticks to either <see cref="TrueNode"/> or <see cref="FalseNode"/>.
    /// Returns <see cref="NodeState.Failure"/> when the condition is false and no
    /// <see cref="FalseNode"/> is provided.
    /// </summary>
    public class ConditionalNode : IAbilityNode, INodeContainer
    {
        private readonly Func<AbilityGraphContext, bool> _condition;
        private readonly IAbilityNode _trueNode;
        private readonly IAbilityNode _falseNode;

        private bool _evaluated;
        private bool _conditionResult;

        public IAbilityNode[] GetChildren()
        {
            if (_falseNode != null) return new[] { _trueNode, _falseNode };
            return new[] { _trueNode };
        }

        public IAbilityNode WithChild(IAbilityNode oldChild, IAbilityNode newChild)
        {
            IAbilityNode trueNode = _trueNode == oldChild ? newChild : _trueNode;
            IAbilityNode falseNode = _falseNode == oldChild ? newChild : _falseNode;
            return new ConditionalNode(_condition, trueNode, falseNode);
        }

        /// <param name="condition">Predicate evaluated once on the first tick.</param>
        /// <param name="trueNode">Node executed when the condition is true.</param>
        /// <param name="falseNode">Node executed when the condition is false. Pass null to return Failure instead.</param>
        public ConditionalNode(Func<AbilityGraphContext, bool> condition, IAbilityNode trueNode, IAbilityNode falseNode = null)
        {
            _condition = condition;
            _trueNode = trueNode;
            _falseNode = falseNode;
        }

        public NodeState Tick(AbilityGraphContext context, float deltaTime)
        {
            if (!_evaluated)
            {
                _conditionResult = _condition(context);
                _evaluated = true;
            }

            if (_conditionResult)
                return _trueNode.Tick(context, deltaTime);

            if (_falseNode != null)
                return _falseNode.Tick(context, deltaTime);

            return NodeState.Failure;
        }
    }
}
