using System;
using Seyren.System.Damages;
using Seyren.System.Units;

namespace Seyren.System.Abilities
{
    /// <summary>
    /// Deals damage to a selected target on the first tick, then returns Success.
    /// </summary>
    public class DealDamageNode : IAbilityNode
    {
        private readonly float _amount;
        private readonly DamageType _damageType;
        private readonly TriggerType _triggerType;
        private readonly Func<AbilityGraphContext, IUnit> _targetSelector;
        private bool _fired;

        public DealDamageNode(float amount, DamageType damageType, Func<AbilityGraphContext, IUnit> targetSelector, TriggerType triggerType = TriggerType.All)
        {
            _amount = amount;
            _damageType = damageType;
            _targetSelector = targetSelector;
            _triggerType = triggerType;
        }

        public NodeState Tick(AbilityGraphContext context, float deltaTime)
        {
            if (_fired) return NodeState.Success;

            IUnit target = _targetSelector(context);
            if (target != null)
                DamageEngine.DamageTarget(context.caster, target, _amount, _damageType, _triggerType);

            _fired = true;
            return NodeState.Success;
        }
    }
}
