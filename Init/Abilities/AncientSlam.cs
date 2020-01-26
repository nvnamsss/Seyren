using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.EventData;
using Base2D.System.UnitSystem.Projectiles;
using Base2D.System.UnitSystem.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base2D.Init.Abilities
{
    public class AncientSlam : ActiveAbility
    {
        public static readonly int Id = 0x65678301;
        private Unit unit;
        private Sprite sprite;
        private RuntimeAnimatorController controller;
        private Dictionary<Unit, int> hitList;
        private static string ancientEnergyPath = "Effect/AncientEnergy/AncientEnergy";

        public AncientSlam(Unit u) : base(u, 2, 10, 1)
        {
            unit = u;
            BaseCoolDown = 10;
            BaseCastingTime = 2;
            controller = Resources.Load<RuntimeAnimatorController>(ancientEnergyPath);
            hitList = new Dictionary<Unit, int>();

            Casting += (sender, e) =>
            {
                unit.Action.Animator.SetBool("Spell", true);
            };

            Casted += (sender) =>
            {
                unit.Action.Animator.SetBool("Spell", false);
            };
        }

        public GameObject Create(Vector2 location, Quaternion rotation)
        {
            return null;
        }

        public void Create()
        {
            float angle = 180;
            for (int loop = 0; loop < 7; loop++)
            {
                float rad = (float)(angle * Mathf.Deg2Rad);
                Vector2 location = unit.transform.position;
                location.y -= 1;
                Quaternion rotation = unit.transform.rotation;
                Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

                MissileProjectile missile = MissileProjectile.Create("Ancient Slam - Energy",
                    location,
                    rotation,
                    sprite,
                    controller,
                    10,
                    2);
                missile.Condition = HitCondition;
                missile.BaseHitDelay = 0;
                //missile.Collider.size = new Vector2(0.86f, 0.86f);
                missile.Direction = direction;
                missile.transform.localScale = new Vector3(3, 3, 1);
                missile.MaxHit = 100000;
                missile.Owner = unit;
                missile.OnHit += (sender, e) =>
                {
                    Unit u = e.GetComponent<Unit>();
                    hitList.Add(u, 0);
                    u.Damage(sender.Owner, System.DamageSystem.DamageType.Physical);
                };
                angle = angle - 30;
            }
        }

        private void HitCondition(Projectile projectile, ConditionEventArgs<GameObject> e)
        {
            Unit u = e.Object.GetComponent<Unit>();
            if (u == null)
            {
                e.Match = false;
                return;
            }

            if (hitList.ContainsKey(u))
            {
                e.Match = false;
                return;
            }

            e.Match = projectile.Owner.IsEnemy(u);
        }

        protected override void DoCastAbility()
        {
            unit.Action.Animator.SetBool("Spell", false);

            Create();
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
