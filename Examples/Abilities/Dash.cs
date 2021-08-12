using Seyren.System.Abilities;
using Seyren.System.Actions;
using Seyren.System.Units.Dummies;
using Seyren.System.Units;
using System.Collections;
using UnityEngine;
using Seyren.System.Generics;

namespace Seyren.Examples.Abilities
{
    public class Dash : InstantAbility
    {
        public static readonly int Id = 0x68658301;
        public ActionConditionHandler RunCondition { get; }


        public Dash(Unit u) : base(1, 1)
        {
            RunCondition += (s) =>
            {
                return true;
            };
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

        protected override void onCast()
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
