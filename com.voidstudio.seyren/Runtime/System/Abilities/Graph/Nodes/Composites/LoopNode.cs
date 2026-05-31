using System;

namespace Seyren.System.Abilities
{
    /// <summary>
    /// Controls how a <see cref="LoopNode"/> decides when to stop repeating.
    /// </summary>
    public enum LoopMode
    {
        /// <summary>Repeats the child indefinitely until it returns Failure.</summary>
        Forever,
        /// <summary>Repeats the child a fixed number of times.</summary>
        Count,
        /// <summary>Repeats the child for a total wall-clock duration.</summary>
        Duration,
        /// <summary>Fires the child once per interval tick (regardless of child duration).</summary>
        Interval
    }

    /// <summary>
    /// Repeats a child node according to the chosen <see cref="LoopMode"/>.
    /// A fresh child is created via <paramref name="childFactory"/> at the start of
    /// each cycle so that internal state is guaranteed to be reset.
    /// </summary>
    public class LoopNode : IAbilityNode
    {
        private readonly Func<IAbilityNode> _childFactory;
        private readonly LoopMode _mode;
        private readonly int _maxCount;
        private readonly float _duration;
        private readonly float _interval;

        // Exposed for graph-level cloning (e.g. MultishotModifier).
        internal Func<IAbilityNode> ChildFactory => _childFactory;
        internal LoopMode Mode => _mode;
        internal int MaxCount => _maxCount;
        internal float Duration => _duration;
        internal float Interval => _interval;

        private IAbilityNode _child;
        private float _timer;
        private int _currentCount;
        private float _intervalTimer;

        /// <param name="mode">Termination mode.</param>
        /// <param name="childFactory">Factory called once per cycle to produce a fresh child node.</param>
        /// <param name="count">Required for <see cref="LoopMode.Count"/>: total iterations.</param>
        /// <param name="duration">Required for <see cref="LoopMode.Duration"/>: total seconds.</param>
        /// <param name="interval">Required for <see cref="LoopMode.Interval"/>: seconds between each child invocation.</param>
        public LoopNode(LoopMode mode, Func<IAbilityNode> childFactory, int count = 0, float duration = 0f, float interval = 0f)
        {
            _childFactory = childFactory;
            _mode = mode;
            _maxCount = count;
            _duration = duration;
            _interval = interval;
            _child = _childFactory();
        }

        public NodeState Tick(AbilityGraphContext context, float deltaTime)
        {
            _timer += deltaTime;

            if (_mode == LoopMode.Duration && _timer >= _duration)
                return NodeState.Success;

            if (_mode == LoopMode.Interval)
            {
                _intervalTimer += deltaTime;
                if (_intervalTimer < _interval)
                    return NodeState.Running;
                _intervalTimer -= _interval;
            }

            NodeState result = _child.Tick(context, deltaTime);

            if (result == NodeState.Failure)
                return NodeState.Failure;

            if (result == NodeState.Success)
            {
                _currentCount++;

                if (_mode == LoopMode.Count && _currentCount >= _maxCount)
                    return NodeState.Success;

                _child = _childFactory();
            }

            return NodeState.Running;
        }
    }
}
