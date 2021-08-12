using Seyren.System.Abilities;
using Seyren.System.Actions;
using Seyren.System.Units.Projectiles;
using Seyren.System.Units;
using System.Collections;
using UnityEngine;
using Seyren.System.Generics;

namespace Seyren.Examples.Abilities
{
    public class MagicFlame : ActiveAbility
    {
        public static readonly int Id = 0x77707601;
        private static string magicFlamePath = "Effect/MagicFlame/MagicFlame";
        
        private bool actionRun;
        private GameObject go;
        public ActionConditionHandler RunCondition { get; }


        public MagicFlame(Unit u) : base(1)
        {

        }

        protected override void DoWhenCasting()
        {
            throw new global::System.NotImplementedException();
        }

        protected override void DoCastAbility()
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
    }
}
