using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Projectiles;
using Base2D.System.UnitSystem.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base2D.Init.Abilities
{
    public class MinotaurHello1 : Ability
    {
        public static readonly int Id = 0x65686501;
        private Unit unit;
        private Sprite sprite;
        private RuntimeAnimatorController controller;
        private Dictionary<Unit, int> hitList;
        public MinotaurHello1(Unit u) : base(u, u.AttackSpeed, 0.1f, 1)
        {
            unit = u;
            BaseCoolDown = unit.AttackSpeed;
            BaseCastingTime = 0.1f;
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

            unit.Action.Animator.SetTrigger("attack1");
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

        public GameObject Create(Vector2 location, Quaternion rotation)
        {
            SwordProjectile slash = SwordProjectile.Create("MinotaurSlash",
                location,
                rotation,
                sprite,
                controller,
                0,
                0.5f);
            slash.HitDelay = 0;
            slash.Collider.isTrigger = true;
            slash.Collider.autoTiling = true;
            slash.Collider.size = new Vector2(0.86f, 0.86f);
            slash.transform.localScale = new Vector3(3, 3, 1);
            slash.MaxHit = 100;
            slash.Owner = unit;
            slash.OnHit += (sender, e) =>
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

            return slash.gameObject;
        }

        protected override void DoCastAbility()
        {
            unit.Action.Animator.SetBool("Attack", false);

            Vector2 location = unit.transform.position;
            Quaternion rotation = unit.transform.rotation;

            location = location + (Vector2)(rotation * new Vector2(1,1));
            location.y -= 1;

            Create(location, rotation);

            unit.Action.Type = System.ActionSystem.ActionType.None;
            TimeCoolDownLeft = BaseCoolDown;
        }

        protected override bool Condition()
        {
            throw new NotImplementedException();
        }
    }
}
