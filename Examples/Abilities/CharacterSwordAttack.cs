using Seyren.System.Abilities;
using Seyren.System.Units.Projectiles;
using Seyren.System.Units;
using System;
using System.Collections;
using UnityEngine;
using Seyren.System.Generics;
using Seyren.System.Actions;
using Seyren.Examples.Actions;

namespace Seyren.Examples.Abilities
{
    public class CharacterSwordAttack : ActiveAbility
    {
        public static readonly int Id = 0x67658401;
        public Unit unit;
        private Sprite sprite;
        public CharacterSwordAttack(Unit u) : base(1)
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
