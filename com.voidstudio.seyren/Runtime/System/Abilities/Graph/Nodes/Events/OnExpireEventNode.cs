namespace Seyren.System.Abilities
{
    /// <summary>
    /// Waits until the <c>"cancel"</c> event is fired on the context (i.e. the
    /// ability instance is cancelled / expires), then executes <paramref name="child"/>.
    /// </summary>
    public class OnExpireEventNode : EventNode
    {
        public OnExpireEventNode(IAbilityNode child) : base(child)
        {
        }

        protected override void RegisterEvent(AbilityGraphContext context)
        {
            context.SubscribeEvent("cancel", MarkFired);
        }
    }
}
