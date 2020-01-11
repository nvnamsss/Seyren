using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Projectiles;
using Base2D.System.UnitSystem.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base2D.Init.Abilities
{
    public class MinotaurSwordAttack1 : Ability
    {
        public static readonly int Id = 0x67658402;
        private Unit unit;
        private Sprite sprite;
        private RuntimeAnimatorController controller;
        private Dictionary<Unit, int> hitList;
        public MinotaurSwordAttack1(Unit u)
        {
            unit = u;
            BaseCoolDown = 10 / unit.AttackSpeed;
            BaseCastingTime = 0.1f;
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
            unit.Action.Type = System.ActionSystem.ActionType.Attack;
            unit.Action.Animator.SetTrigger("attack1");
            unit.StartCoroutine(Casting(Time.deltaTime, BaseCastingTime));

            return true;
        }
        public override GameObject Create(Vector2 location, Quaternion rotation)
        {
            MissileProjectile missile = MissileProjectile.Create("Minotaur Sword Attack1",
                location,
                rotation,
                sprite,
                controller,
                0,
                0.1f);
            missile.HitDelay = 0;
            missile.Collider.isTrigger = true;
            missile.Collider.size = new Vector2(3.3f, 3.5f);
            missile.Collider.offset = new Vector2(3f, -0.5f);
            missile.transform.localScale = new Vector3(1, 1, 1);
            missile.MaxHit = 1000;
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

        protected override void DoCastAbility()
        {
            unit.Action.Animator.SetTrigger("end-atk1");
            Vector2 location = unit.transform.position;
            Quaternion rotation = unit.transform.rotation;

            location = location + (Vector2)(rotation * Vector2.left * 2);
            Create(location, rotation);
            unit.Action.Type = System.ActionSystem.ActionType.None;
            TimeCoolDownLeft = BaseCoolDown;
            unit.StartCoroutine(StartCoolDown(Time.deltaTime, BaseCoolDown));
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

