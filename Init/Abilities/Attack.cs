using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Projectiles;
using Base2D.System.UnitSystem.Units;
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
        public Unit unit;
        public Sprite Sprite;
        public ArrowProjectile Projectile;
        public Attack(Unit u)
        {
            Sprite = ProjectileCollection.Cut;
            unit = u;
            Projectile = ArrowProjectile.Create("attack", Vector3.zero,
                Quaternion.Euler(0, 0, 0),
                ProjectileCollection.Cut,
                ProjectileCollection.CutController,
                1, 1, 0.1f);
        }

        public override GameObject Create(Vector2 location, Quaternion rotation)
        {
            var arrow = ArrowProjectile.Create("attack", location,
                rotation,
                ProjectileCollection.Cut,
                ProjectileCollection.CutController,
                1, 1, 0.1f);
            arrow.Owner = unit;

            arrow.OnHit += (sender, e) =>
            {
                Unit u = e.GetComponent<Unit>();

                if (u != null && unit.IsEnemy(u))
                {
                    u.Damage(arrow.Owner, System.DamageSystem.DamageType.Physical);
                }
            };

            return arrow.gameObject;
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
