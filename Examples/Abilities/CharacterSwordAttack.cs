using Seyren.System.Abilities;
using Seyren.System.Units.Projectiles;
using Seyren.System.Units;
using System;
using System.Collections;
using UnityEngine;

namespace Seyren.Example.Abilities
{
    public class CharacterSwordAttack : ActiveAbility
    {
        public static readonly int Id = 0x67658401;
        public Unit unit;
        private Sprite sprite;
        private RuntimeAnimatorController controller;
        public CharacterSwordAttack(Unit u) : base(u, 0.25f, 10 / u.Attribute.AttackSpeed, 1)
        {
            unit = u;
            BaseCoolDown = 10 / unit.Attribute.AttackSpeed;
            BaseCastTime = 0.25f;

            Casting += (sender, e) =>
            {

            };

            CastCompleted += (sender) =>
            {

            };
        }
        public GameObject Create(Vector2 location, Quaternion rotation)
        {
            MissileProjectile missile = MissileProjectile.Create("CharacterSwordAttack",
                location,
                rotation,
                sprite,
                controller,
                0,
                0.4f);
            //missile.Collider.size = new Vector2(2.4f, 2.4f);
            missile.MaxHit = 10000000;
            missile.Owner = unit;
            missile.OnHit += (sender, e) =>
            {
                Unit u = e.GetComponent<Unit>();
                if (u != null && unit.IsEnemy(u))
                {
                    Debug.Log("Damage");
                    u.Damage(missile.Owner, System.Damages.DamageType.Physical);
                }
                else
                {
                    Debug.Log("Cannot damage");
                }
            };

            return missile.gameObject;
        }



        protected override void DoCastAbility()
        {
            //unit.Action.Animator.SetTrigger("end-atk1");
            Vector2 location = unit.transform.position;
            Quaternion rotation = unit.transform.rotation;

            location = location + (Vector2)(rotation * Vector2.left * 2);
            Create(location, rotation);
            unit.Action.Type = System.Actions.ActionType.None;
            CooldownRemaining = BaseCoolDown;
        }

        protected override bool Condition()
        {
            return !Active ||
                CooldownRemaining > 0 ||
                IsCasting ||
                unit.Action.Type == System.Actions.ActionType.CastAbility ||
                unit.Action.Type == System.Actions.ActionType.Attack;
        }

        protected override bool UnlockCondition()
        {
            throw new NotImplementedException();
        }
    }
}
