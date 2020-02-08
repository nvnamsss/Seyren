using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Projectiles;
using Base2D.System.UnitSystem.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base2D.Init.Abilities
{
    public class AndrasAttack : ActiveAbility
    {
        public static readonly int Id = 0x65686501;
        private Unit unit;
        private Sprite sprite;
        private RuntimeAnimatorController controller;
        private Dictionary<Unit, int> hitList;
        private static string ancientEnergyPath = "Effect/AncientEnergy/AncientEnergy";
        public AndrasAttack(Unit u) : base(u, 0.4f, u.Attribute.AttackSpeed, 1)
        {
            unit = u;
            BaseCoolDown = unit.Attribute.AttackSpeed;
            BaseCastTime = 0.4f;
            controller = Resources.Load<RuntimeAnimatorController>(ancientEnergyPath);
            hitList = new Dictionary<Unit, int>();
        }

        public GameObject Create(Vector2 location, Quaternion rotation)
        {
            MissileProjectile missile = MissileProjectile.Create("AndrasAttack",
                location,
                rotation,
                sprite,
                controller,
                0,
                0.5f);
            missile.BaseHitDelay = 0;
            missile.Collider.isTrigger = true;
            //missile.Collider.autoTiling = true;
            //missile.Collider.size = new Vector2(0.86f, 0.86f);
            missile.transform.localScale = new Vector3(3, 3, 1);
            missile.MaxHit = 100;
            missile.Owner = unit;
            missile.OnHit += (sender, e) =>
            {
                Unit u = e.GetComponent<Unit>();
                if (u == null)
                {
                    return;
                }

                Debug.Log(u);
                if (hitList.ContainsKey(u))
                {
                    return;
                }
                else
                {
                    hitList.Add(u, 0);
                }

                if (sender.Owner.IsEnemy(u))
                {
                    u.Damage(sender.Owner, System.DamageSystem.DamageType.Physical);
                }
                else
                {
                    sender.ResetHit();
                    Debug.Log("Cannot damage");
                }
            };

            return missile.gameObject;
        }

        protected override void DoCastAbility()
        {
            unit.Action.Animator.SetBool("Attack", false);

            Vector2 location = unit.transform.position;
            Quaternion rotation = unit.transform.rotation;

            location = location + (Vector2)(rotation * Vector2.left * 3);
            location.y -= 1;

            Create(location, rotation);

            unit.Action.Type = System.ActionSystem.ActionType.None;
            CooldownRemaining = BaseCoolDown;
        }

        protected override bool Condition()
        {
            return !Active ||
                CooldownRemaining > 0 ||
                IsCasting ||
                unit.Action.Type == System.ActionSystem.ActionType.CastAbility ||
                unit.Action.Type == System.ActionSystem.ActionType.Attack;
        }
    }
}
