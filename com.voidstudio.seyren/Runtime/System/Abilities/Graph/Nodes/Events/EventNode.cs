namespace Seyren.System.Abilities
{
    /// <summary>
    /// Abstract base for nodes that wait for a named context event before
    /// executing a child node. Subclasses call <see cref="RegisterEvent"/> to
    /// subscribe to the specific event they care about.
    ///
    /// Lifecycle:
    /// 1. First tick: call <see cref="RegisterEvent"/>, return Running.
    /// 2. Subsequent ticks while event not yet fired: return Running.
    /// 3. Once the event fires: tick <paramref name="_child"/> each frame,
    ///    propagating its result.
    /// </summary>
    public abstract class EventNode : IAbilityNode
    {
        private readonly IAbilityNode _child;
        private bool _subscribed;
        private bool _fired;

        protected EventNode(IAbilityNode child)
        {
            _child = child;
        }

        /// <summary>
        /// Subscribe to the relevant context event so that when it fires,
        /// <c>_fired</c> is set to <c>true</c>. Called exactly once on the first tick.
        /// </summary>
        protected abstract void RegisterEvent(AbilityGraphContext context);

        /// <summary>Sets _fired = true. Call this from inside RegisterEvent's callback.</summary>
        protected void MarkFired() => _fired = true;

        public NodeState Tick(AbilityGraphContext context, float deltaTime)
        {
            if (!_subscribed)
            {
                RegisterEvent(context);
                _subscribed = true;
            }

            if (!_fired)
                return NodeState.Running;

            return _child.Tick(context, deltaTime);
        }
    }
}
