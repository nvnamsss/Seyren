namespace Seyren.System.Abilities
{
    /// <summary>
    /// Waits for a fixed duration before succeeding.
    /// </summary>
    public class WaitNode : IAbilityNode
    {
        private readonly float _duration;
        private float _elapsed;

        public WaitNode(float duration)
        {
            _duration = duration;
        }

        public NodeState Tick(AbilityGraphContext context, float deltaTime)
        {
            _elapsed += deltaTime;
            return _elapsed >= _duration ? NodeState.Success : NodeState.Running;
        }
    }
}
