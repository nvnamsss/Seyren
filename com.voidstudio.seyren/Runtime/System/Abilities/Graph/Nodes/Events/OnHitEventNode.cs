namespace Seyren.System.Abilities
{
    /// <summary>
    /// Waits until the <c>"onHit"</c> event is fired on the context, then
    /// executes <paramref name="child"/>.
    /// </summary>
    public class OnHitEventNode : EventNode
    {
        public OnHitEventNode(IAbilityNode child) : base(child)
        {
        }

        protected override void RegisterEvent(AbilityGraphContext context)
        {
            context.SubscribeEvent("onHit", MarkFired);
        }
    }
}
