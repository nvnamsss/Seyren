using Seyren.System.Abilities;
using Seyren.System.Units.Dummies;
using Seyren.System.Units;
using UnityEngine;
using Seyren.System.Generics;
using Seyren.System.Actions;
using System.Collections.Generic;

namespace Seyren.Examples.Abilities
{
    public class DevotionAura : AuraAbility
    {
        public static readonly int Id = 0x68696501;
        HashSet<IUnit> affect;
        Vector3 position;
        public DevotionAura(Unit caster, float aoe, int level) : base(aoe, level)
        {
            affect = new HashSet<IUnit>();
        }

        public override IAction Action(IUnit by)
        {
            throw new global::System.NotImplementedException();
        }

        public override IAction Action(IUnit by, IUnit target)
        {
            throw new global::System.NotImplementedException();
        }

        public override IAction Action(IUnit by, Vector3 target)
        {
            throw new global::System.NotImplementedException();
        }

        public override Ability Clone()
        {
            throw new global::System.NotImplementedException();
        }

        protected override void AuraInterval()
        {
           List<IUnit> picked = TestAuraAbility.universe.PickUnitsInRange(position, aoe);
           
        }

        protected override Error Condition(IUnit by)
        {
            return null;
        }

        protected override Error Condition(IUnit by, IUnit target)
        {
            return null;
        }

        protected override Error Condition(IUnit by, Vector3 target)
        {
            return null;
        }
    }
}
