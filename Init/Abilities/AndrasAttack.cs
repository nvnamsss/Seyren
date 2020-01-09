using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Projectiles;
using Base2D.System.UnitSystem.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base2D.Init.Abilities
{
    public class AndrasAttack : Ability
    {
        public static readonly int Id = 0x65686501;
        private Unit unit;
        private Sprite sprite;
        private RuntimeAnimatorController controller;
        private Dictionary<Unit, int> hitList;
        public AndrasAttack(Unit u)
        {
            unit = u;
            BaseCoolDown = unit.AttackSpeed;
            BaseCastingTime = 0.4f;
            controller = ProjectileCollection.AncientEnergyController;
            hitList = new Dictionary<Unit, int>();
        }
        public override bool Cast()
        {
            if (TimeCoolDownLeft > 0 ||
                IsCasting ||
                unit.Action.Type == System.ActionSystem.ActionType.CastAbility ||
                unit.Action.Type == System.ActionSystem.ActionType.Attack)
            {
                return false;
            }

            hitList.Clear();
            unit.Action.Type = System.ActionSystem.ActionType.CastAbility;

            unit.Action.Animator.SetBool("Attack", true);
            TimeCastingLeft = BaseCastingTime;
            unit.StartCoroutine(Casting(Time.deltaTime, BaseCastingTime));
            return true;
        }


        IEnumerator Casting(float timeDelay, float timeCasting)
        {
            IsCasting = true;
            yield return new WaitForSeconds(timeDelay);
            TimeCastingLeft = timeCasting - timeDelay;

            while (TimeCastingLeft >= 0)
            {
                yield return new WaitForSeconds(timeDelay);
                TimeCastingLeft -= timeDelay;
            }

            if (IsCasting)
            { 
                IsCasting = false;
                DoCastAbility();
            }

        }

        public override GameObject Create(Vector2 location, Quaternion rotation)
        {
            MissileProjectile missile = MissileProjectile.Create("AndrasAttack",
                location,
                rotation,
                sprite,
                controller,
                0,
                0.5f);
            missile.HitDelay = 0;
            missile.Collider.isTrigger = true;
            missile.Collider.autoTiling = true;
            missile.Collider.size = new Vector2(0.86f, 0.86f);
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
            TimeCoolDownLeft = BaseCoolDown;
            unit.StartCoroutine(StartCoolDown(Time.deltaTime, BaseCoolDown));
        }
    }
}
