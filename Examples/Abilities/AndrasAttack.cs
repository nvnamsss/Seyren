using Seyren.System.Abilities;
using Seyren.System.Units.Projectiles;
using Seyren.System.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seyren.System.Generics;
using Seyren.System.Actions;

namespace Seyren.Examples.Abilities
{
    public class AndrasAttack : ActiveAbility
    {
        public static readonly int Id = 0x65686501;
        private Unit unit;
        private RuntimeAnimatorController controller;
        private Dictionary<Unit, int> hitList;

        public AndrasAttack(int level) : base(level)
        {
        }

        public override IAction Action(Unit by)
        {
            throw new NotImplementedException();
        }

        public override IAction Action(Unit by, Unit target)
        {
            throw new NotImplementedException();
        }

        public override IAction Action(Unit by, Vector3 target)
        {
            throw new NotImplementedException();
        }

        protected override Error Condition(Unit by)
        {
            throw new NotImplementedException();
        }

        protected override Error Condition(Unit by, Unit target)
        {
            throw new NotImplementedException();
        }

        protected override Error Condition(Unit by, Vector3 target)
        {
            throw new NotImplementedException();
        }

        protected override void DoCastAbility()
        {
            throw new NotImplementedException();
        }

        protected override void DoWhenCasting()
        {
            throw new NotImplementedException();
        }
    }
}
