namespace Seyren.Gameplay
{
    /// <summary>
    /// Represents a single step in the core gameplay loop pipeline.
    /// </summary>
    public interface ICoreStep
    {
        void Execute(GameContext ctx);
        bool IsComplete(GameContext ctx);
    }
}
