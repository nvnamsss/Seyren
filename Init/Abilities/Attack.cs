using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base2D.Init.Abilities
{
    public class Attack : Ability
    {
        public static readonly int Id = 0x64848401;
        public Attack(GameObject unit)
        {
            this.BaseUnit = unit;
        } 
        protected override void DoAnimation()
        {
        }

        protected override void DoCastAbility()
        {
            Vector3 direction = ObjectTarget.transform.position - BaseUnit.transform.position;
            ArrowProjectile.Create("attack",
                BaseUnit.transform.position,
                Utils.RotationUtil.EulerAngleBetween(BaseUnit.transform.position, ObjectTarget.transform.position),
                ProjectileCollection.Mana,
                10,
                1);
        }

        protected override void DoSomeThingIfCannotCasting()
        {
        }

        protected override void Initialize()
        {

        }

        protected override bool SpecialBreakAbility()
        {
            return false;
        }
    }
}
