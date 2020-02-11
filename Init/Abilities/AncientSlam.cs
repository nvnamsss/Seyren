using Base2D.System.AbilitySystem;
using Base2D.System.ActionSystem;
using Base2D.System.UnitSystem.EventData;
using Base2D.System.UnitSystem.Projectiles;
using Base2D.System.UnitSystem.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base2D.Init.Abilities
{
    public class AncientSlam : ActiveAbility, IAction
    {
        public static readonly int Id = 0x65678301;
        private Sprite sprite;
        private RuntimeAnimatorController controller;
        private Dictionary<Unit, int> hitList;
        private static string ancientEnergyPath = "Effect/AncientEnergy/AncientEnergy";

        public ActionConditionHandler RunCondition { get; }

        public ActionType ActionType => ActionType.CastAbility;
        public event ActionHandler ActionStart;
        public event ActionHandler ActionEnd;
        private GameObject go;
        private bool actionRun;

        public AncientSlam(Unit u) : base(u, 2, 10, 1)
        {
            BaseCoolDown = 10;
            BaseCastTime = 2;
            controller = Resources.Load<RuntimeAnimatorController>(ancientEnergyPath);
            hitList = new Dictionary<Unit, int>();

            Casting += (sender, e) =>
            {
                Caster.Action.Animator.SetBool("Spell", true);
            };

            Casted += (sender) =>
            {
                Caster.Action.Animator.SetBool("Spell", false);
            };

            ActionStart += (s) =>
            {
                actionRun = true;
                Caster.Action.Animator.SetBool("Spell", true);
            };

            ActionEnd += (s) =>
            {
                actionRun = false;
                Caster.Action.Animator.SetBool("Spell", false);
            };

            RunCondition = (action) =>
            {
                if (action.ActionType == ActionType.CastAbility)
                {
                    return false;
                }
                return true;
            };

            RunCondition += (action) =>
            {
                return false;
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
                float rad = (angle * Mathf.Deg2Rad);
                Vector2 location = Caster.transform.position;
                location.y -= 1;
                Quaternion rotation = Caster.transform.rotation;
                Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

                MissileProjectile missile = MissileProjectile.Create("Ancient Slam - Energy",
                    location,
                    rotation,
                    sprite,
                    controller,
                    10,
                    2);
                missile.HitCondition = HitCondition;
                missile.BaseHitDelay = 0;
                //missile.Collider.size = new Vector2(0.86f, 0.86f);
                missile.Direction = direction;
                missile.transform.localScale = new Vector3(3, 3, 1);
                missile.MaxHit = 100000;
                missile.Owner = Caster;
                missile.OnHit += (sender, e) =>
                {
                    Unit u = e.GetComponent<Unit>();
                    hitList.Add(u, 0);
                    u.Damage(sender.Owner, System.DamageSystem.DamageType.Physical);
                };
                angle = angle - 30;
            }
        }

        private bool HitCondition(Projectile projectile, GameObject obj)
        {
            Unit u = obj.GetComponent<Unit>();
            if (u == null)
            {
                return false;
            }

            if (hitList.ContainsKey(u))
            {
                return false;
            }

            return projectile.Owner.IsEnemy(u);
        }

        protected override void DoCastAbility()
        {
            Create();
            if (actionRun)
                Revoke();
        }

        protected override bool Condition()
        {
            return !(!Active ||
                CooldownRemaining > 0 ||
                IsCasting);
        }

        public void Invoke()
        {
            bool cast = Cast();
            if (cast)
            {
                ActionStart?.Invoke(this);
            }
        }

        public void Revoke()
        {
            ActionEnd?.Invoke(this);
            Caster.StopCoroutine(castCoroutine);
        }

        protected override bool UnlockCondition()
        {
            return true;
        }
    }
}
