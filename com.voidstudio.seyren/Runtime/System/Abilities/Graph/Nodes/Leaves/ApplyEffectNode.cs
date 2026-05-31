using System;
using Seyren.System.Buffs;
using Seyren.System.Units;

namespace Seyren.System.Abilities
{
    /// <summary>
    /// Applies a buff to a selected target on the first tick, then returns Success.
    /// </summary>
    public class ApplyEffectNode : IAbilityNode
    {
        private readonly Func<AbilityGraphContext, IUnit> _targetSelector;
        private readonly IBuff _effect;
        private bool _fired;

        public ApplyEffectNode(Func<AbilityGraphContext, IUnit> targetSelector, IBuff effect)
        {
            _targetSelector = targetSelector;
            _effect = effect;
        }

        public NodeState Tick(AbilityGraphContext context, float deltaTime)
        {
            if (_fired) return NodeState.Success;

            IUnit target = _targetSelector(context);
            if (target != null)
                _effect.ApplyBuffToUnit(target);

            _fired = true;
            return NodeState.Success;
        }
    }
}
