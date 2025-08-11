using System;
using UnityEngine;

namespace Seyren.Gameplay
{
    /// <summary>
    /// Core step that executes a dynamic action when conditions are met.
    /// </summary>
    public class DynamicCoreStep : ICoreStep
    {
        private readonly Action<GameContext> _action;
        private readonly Func<GameContext, bool> _canStartCondition;
        private readonly Func<GameContext, bool> _isCompleteCondition;
        private bool _hasExecuted;

        public DynamicCoreStep(
            Action<GameContext> action, 
            Func<GameContext, bool> canStartCondition = null,
            Func<GameContext, bool> isCompleteCondition = null)
        {
            _action = action;
            _canStartCondition = canStartCondition;
            _isCompleteCondition = isCompleteCondition;
            _hasExecuted = false;
        }

        public bool CanStart(GameContext ctx)
        {
            return _canStartCondition?.Invoke(ctx) ?? true;
        }

        public void Execute(GameContext ctx)
        {
            if (_hasExecuted) return;
            
            _action?.Invoke(ctx);
            _hasExecuted = true;
        }

        public bool IsComplete(GameContext ctx)
        {
            bool completed = _isCompleteCondition?.Invoke(ctx) ?? _hasExecuted;
            return completed;
        }
    }
}