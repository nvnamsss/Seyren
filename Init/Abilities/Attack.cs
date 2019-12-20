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
        public Sprite Sprite;
        public ArrowProjectile Projectile;
        public Attack(GameObject unit)
        {
            Sprite = ProjectileCollection.Cut;
            Projectile = ArrowProjectile.Create("attack", Vector3.zero,
                Quaternion.Euler(0, 0, 0),
                ProjectileCollection.Cut,
                ProjectileCollection.CutController,
                1, 1, 0.1f);
        }

        public override GameObject Create(Vector2 location, Quaternion rotation)
        {
            var go = ArrowProjectile.Create("attack", location,
                rotation,
                ProjectileCollection.Cut,
                ProjectileCollection.CutController,
                1, 1, 0.1f).gameObject;

            return go;
        }

        protected override void DoAnimation()
        {
        }

        protected override void DoCastAbility()
        {
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
