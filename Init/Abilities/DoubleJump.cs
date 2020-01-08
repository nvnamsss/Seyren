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
        public GameObject Effect;
        public DoubleJump(Unit u)
        {
            Effect = ProjectileCollection.Cyclone;
            unit = u;
            BaseCoolDown = 1.0f;
        }

        public override bool Cast()
        {
            if (TimeCoolDownLeft > 0)
            {
                return false;
            }

            Vector2 force = new Vector2(0, 0.5f * unit.JumpSpeed);
            unit.Body.AddForce(force, ForceMode2D.Impulse);
            GameObject effect = Instantiate<GameObject>(Effect, unit.transform.position, unit.transform.rotation);
            Destroy(effect, 0.5f);
            TimeCoolDownLeft = BaseCoolDown;
            unit.StartCoroutine(StartCoolDown(Time.deltaTime, 1));
            return true;
        }

        protected override void Tick(float time)
        {
        }
        public override GameObject Create(Vector2 location, Quaternion rotation)
        {

            return gameObject;
        }

        protected override void DoCastAbility()
        {
            throw new NotImplementedException();
        }

        //protected override void DoAnimation()
        //{
        //}

        //protected override void DoCastAbility()
        //{
        //}

        //protected override void DoSomeThingIfCannotCasting()
        //{
        //}

        //protected override void Initialize()
        //{
        //}

        //protected override bool SpecialBreakAbility()
        //{
        //    return false;
        //}
    }
}
