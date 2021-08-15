using Seyren.System.Abilities;
using Seyren.System.Units.Dummies;
using Seyren.System.Units;
using UnityEngine;
using Seyren.System.Generics;
using Seyren.System.Actions;

namespace Seyren.Examples.Abilities
{
    public class DevotionAura : AuraAbility
    {
        public static readonly int Id = 0x68696501;
        private Dummy dummy;
        public DevotionAura(Unit caster, float aoe, int level) : base(aoe, level)
        {
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
            throw new global::System.NotImplementedException();
        }

        protected override Error Condition(IUnit by)
        {
            throw new global::System.NotImplementedException();
        }

        protected override Error Condition(IUnit by, IUnit target)
        {
            throw new global::System.NotImplementedException();
        }

        protected override Error Condition(IUnit by, Vector3 target)
        {
            throw new global::System.NotImplementedException();
        }

        protected override void onCast()
        {
            throw new global::System.NotImplementedException();
        }
    }
}
