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
            // GameObject go = Resources.Load<GameObject>("DevotionAura");
            // dummy = Dummy.Create<CircleCollider2D>(go);
            // dummy.Owner = Caster;
            // CircleCollider2D collider = dummy.Collider as CircleCollider2D;
            // collider.radius = base.aoe;
            // dummy.Collider.isTrigger = true;
            // dummy.Body.constraints = RigidbodyConstraints2D.FreezeAll;

            // dummy.UnitIn += UnitInCallback;
            // dummy.UnitOut += UnitOutCallback;
        }

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

        protected override void AuraInterval()
        {
            throw new global::System.NotImplementedException();
        }

        protected override Error Condition(Unit by)
        {
            throw new global::System.NotImplementedException();
        }

        protected override Error Condition(Unit by, Unit target)
        {
            throw new global::System.NotImplementedException();
        }

        protected override Error Condition(Unit by, Vector3 target)
        {
            throw new global::System.NotImplementedException();
        }

        protected override void onCast()
        {
            throw new global::System.NotImplementedException();
        }
    }
}
