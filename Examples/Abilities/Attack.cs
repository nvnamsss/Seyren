using System.Collections.Generic;
using Seyren.System.Abilities;
using Seyren.System.Actions;
using Seyren.System.Damages;
using Seyren.System.Generics;
using Seyren.System.States;
using Seyren.System.Units;
using UnityEngine;

namespace Seyren.Examples.Abilities
{
    public class Attack : ActiveAbility
    {
        DamageType damageType;
        TriggerType triggerType;
        List<int> affectedBy;
        public Attack() : base(1)
        {
            cooldownType = CooldownType.WhenCast;
            Targeting = TargetingType.UnitTarget;
            damageType = DamageType.Magical;
            triggerType = TriggerType.All;
            castTime = 1;
            affectedBy = new List<int>(){
                (int)ActionType.Casting, (int)ActionType.Channeling, (int)ActionType.Moving,
            };
        }

        public override IAction Action(IUnit by)
        {
            throw new global::System.NotImplementedException();
        }

        public override IAction Action(IUnit by, IUnit target)
        {
            BaseFloat attackSpeed = by.Attribute.GetBaseFloat("AttackSpeed");
            castTime = 1f;
            int runtime = 1000;
            Action action = new Action((int)ActionType.Attacking, runtime, affectedBy);
            action.ActionStart += (a) => {
                Cast(by);
            };
            action.ActionBroke += (a) => {
                Cancel();
            };
            by.Actions.Run(action);

            return action;
        }

        public override IAction Action(IUnit by, Vector3 target)
        {
            throw new global::System.NotImplementedException();
        }

        public override Ability Clone()
        {
            return new Attack();
        }

        protected override Error Condition(IUnit by)
        {
            if (casting) return new Error("ability is using");
            return null;
        }

        protected override Error Condition(IUnit by, IUnit target)
        {
            if (casting) return new Error("ability is using");
            return null;;
        }

        protected override Error Condition(IUnit by, Vector3 target)
        {
            if (casting) return new Error("ability is using");
            return null;;
        }

        protected override void DoCastAbility()
        {
            Debug.Log("Do cast");
            float damage = abilityTarget.Source.Attribute.GetBaseFloat("AttackDamage").Total;
            DamageEngine.Damage(abilityTarget.Source, abilityTarget.Target, damage, damageType, triggerType);
        }

        protected override void DoWhenCasting()
        {
            // Debug.Log("Do casting");
        }
    }
}