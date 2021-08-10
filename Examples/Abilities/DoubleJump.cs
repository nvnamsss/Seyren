using Seyren.System.Abilities;
using Seyren.System.Actions;
using Seyren.System.Generics;
using Seyren.System.Units;
using UnityEngine;

namespace Seyren.Examples.Abilities
{
    public class DoubleJump : InstantAbility
    {
        public static readonly int Id = 0x74857701;
        public Unit unit;
        public GameObject Effect;
        private static string cyclonePath = "Effect/Cyclone/Cyclone_Effect";
        public DoubleJump(Unit u) : base(1.0f, 1)
        {
            Effect = Resources.Load<GameObject>(cyclonePath);
            unit = u;
            Cooldown = 1.0f;
        }

        protected override void onCast(Unit by)
        {
            throw new global::System.NotImplementedException();
        }

        protected override void onCast(Unit by, Unit target)
        {
            throw new global::System.NotImplementedException();
        }

        protected override void onCast(Unit by, Vector3 target)
        {
            throw new global::System.NotImplementedException();
        }

        protected override void DoCastAbility()
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

        public override long CastTime(Unit unit)
        {
            throw new global::System.NotImplementedException();
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
    }
}
