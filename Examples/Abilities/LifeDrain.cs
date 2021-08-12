using System.Collections.Generic;
using Seyren.System.Abilities;
using Seyren.System.Actions;
using Seyren.System.Generics;
using Seyren.System.Units;
using Seyren.System.Damages;
using UnityEngine;

namespace Seyren.Examples.Abilities
{
    public class LifeDrain : ChannelAbility
    {
        int damage;
        DamageType damageType;
        TriggerType triggerType;
        public LifeDrain(int level) : base(level)
        {
            Targeting = TargetingType.UnitTarget;
            damageType = DamageType.Magical;
            triggerType = TriggerType.All;
        }

        public ActionConditionHandler RunCondition => throw new global::System.NotImplementedException();

        public event GameEventHandler<IAction> ActionStart;
        public event GameEventHandler<IAction> ActionEnd;

        public override IAction Action(Unit by)
        {
            throw new global::System.NotImplementedException();
        }

        public override IAction Action(Unit by, Unit target)
        {
            throw new global::System.NotImplementedException();
        }

        public override IAction Action(Unit by, Vector3 target)
        {
            throw new global::System.NotImplementedException();
        }


        protected override Error Condition(Unit by)
        {
            return null;
        }

        protected override Error Condition(Unit by, Unit target)
        {
            return null;
        }

        protected override Error Condition(Unit by, Vector3 target)
        {
            return null;
        }

        protected override void DoChannelAbility()
        {
            DamageEngine.Damage(abilityTarget.Source, abilityTarget.Target, damage,damageType, triggerType);
        }
    }
}