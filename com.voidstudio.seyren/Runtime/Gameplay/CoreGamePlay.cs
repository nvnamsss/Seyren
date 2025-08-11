using System.Collections.Generic;
using Seyren.System.Common;
using Seyren.Universe;
using UnityEngine;

namespace Seyren.Gameplay
{
    /// <summary>
    /// Manages the sequence of core gameplay steps.
    /// </summary>
    public abstract class CoreGameplay : ILoop
    {
        public GameContext Context { get; private set; }
        protected readonly List<ICoreStep> steps = new List<ICoreStep>();

        public CoreGameplay(GameContext ctx)
        {
            Context = ctx;
            steps = new List<ICoreStep>();
        }

        public CoreGameplay(GameContext ctx, IEnumerable<ICoreStep> steps)
        {
            Context = ctx;
            this.steps.AddRange(steps);
        }

        private int currentIndex = 0;

        public virtual void Loop(ITime time)
        {
            if (currentIndex >= steps.Count)
            {
                return; // All steps completed
            }
            
            ICoreStep currentStep = steps[currentIndex];
            currentStep.Execute(Context);

            if (currentStep.IsComplete(Context))
            {
                Debug.Log($"Step {currentIndex + 1}/{steps.Count} completed.");
                // update the last wave time of the game context
                Context.LastWaveTime = time.CurrentTime;
                currentIndex++;
                return;
            }
        }
    }
}
