using System.Collections.Generic;

namespace Seyren.Gameplay
{
    /// <summary>
    /// Manages the sequence of core gameplay steps.
    /// </summary>
    public class CoreLoop
    {
        private readonly List<ICoreStep> steps = new List<ICoreStep>();

        public CoreLoop(IEnumerable<ICoreStep> steps)
        {
            this.steps.AddRange(steps);
        }

        public void Run(GameContext ctx)
        {
            foreach (var step in steps)
            {
                step.Execute(ctx);
            }
        }
    }
}
