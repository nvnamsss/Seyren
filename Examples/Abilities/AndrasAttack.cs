using Seyren.System.Abilities;
using Seyren.System.Units.Projectiles;
using Seyren.System.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seyren.System.Generics;

namespace Seyren.Examples.Abilities
{
    public class AndrasAttack : ActiveAbility
    {
        public static readonly int Id = 0x65686501;
        private Unit unit;
        private Sprite sprite;
        private RuntimeAnimatorController controller;
        private Dictionary<Unit, int> hitList;

        public AndrasAttack(float castTime, float cooldown, int level) : base(castTime, cooldown, level)
        {
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
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

        protected override void onCast(Unit by)
        {
            throw new NotImplementedException();
        }

        protected override void onCast(Unit by, Unit target)
        {
            throw new NotImplementedException();
        }

        protected override void onCast(Unit by, Vector3 target)
        {
            throw new NotImplementedException();
        }
    }
}
