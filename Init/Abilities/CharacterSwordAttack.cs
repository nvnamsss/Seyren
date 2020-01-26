using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Projectiles;
using Base2D.System.UnitSystem.Units;
using System;
using System.Collections;
using UnityEngine;

namespace Base2D.Init.Abilities
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
            BaseCastingTime = 0.25f;

            Casting += (sender, e) =>
            {

            };

            Casted += (sender) =>
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
                    u.Damage(missile.Owner, System.DamageSystem.DamageType.Physical);
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
            unit.Action.Type = System.ActionSystem.ActionType.None;
            TimeCoolDownLeft = BaseCoolDown;
        }

        protected override bool Condition()
        {
            return !Active ||
                TimeCoolDownLeft > 0 ||
                IsCasting ||
                unit.Action.Type == System.ActionSystem.ActionType.CastAbility ||
                unit.Action.Type == System.ActionSystem.ActionType.Attack;
        }
    }
}
