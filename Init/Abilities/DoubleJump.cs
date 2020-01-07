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
    public class DoubleJump : Ability
    {
        public static readonly int Id = 0x74857701;
        public Unit unit;
        public Sprite Sprite;
        public ArrowProjectile Projectile;
        public DoubleJump(Unit u)
        {
            Sprite = ProjectileCollection.Cut;
            unit = u;
            BaseCoolDown = 1.0f;
        }

        public override bool Cast()
        {
            Debug.Log("Cast Double jump");
            if (TimeCoolDownLeft > 0)
            {
                return false;
            }

            Vector2 force = new Vector2(0, 1.0f * unit.JumpSpeed);
            unit.Body.AddForce(force, ForceMode2D.Impulse);

            return true;
        }
        public override GameObject Create(Vector2 location, Quaternion rotation)
        {

            return gameObject;
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
