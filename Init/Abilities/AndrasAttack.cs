using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Projectiles;
using Base2D.System.UnitSystem.Units;
using System;
using System.Collections;
using UnityEngine;

namespace Base2D.Init.Abilities
{
    public class AndrasAttack : Ability
    {
        public static readonly int Id = 0x65686501;
        private Unit unit;
        private Sprite sprite;
        private RuntimeAnimatorController controller;
        public AndrasAttack(Unit u)
        {
            unit = u;
            BaseCoolDown = unit.AttackSpeed;
            BaseCastingTime = 0.2f;
            sprite = ProjectileCollection.AncientEnergy;
            controller = ProjectileCollection.AncientEnergyController;
        }
        public override bool Cast()
        {
            if (TimeCoolDownLeft > 0 || IsCasting)
            {
                return false;
            }

            unit.Action.Animator.SetBool("Attack", true);
            TimeCastingLeft = BaseCastingTime;
            unit.StartCoroutine(Casting(Time.deltaTime, BaseCastingTime));
            return true;
        }

        IEnumerator Casting(float timeDelay, float timeCasting)
        {
            yield return new WaitForSeconds(timeDelay);
            IsCasting = true;
            TimeCastingLeft = timeCasting;

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
            MissileProjectile missile = MissileProjectile.Create("missile",
            location,
            rotation,
            sprite,
            controller,
            0,
            2);

            missile.MaxHit = 100;
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
            IsCasting = false;
            unit.Action.Animator.SetBool("Attack", false);

            Create(unit.transform.position, unit.transform.rotation);
            TimeCoolDownLeft = BaseCoolDown;
            unit.StartCoroutine(StartCoolDown(Time.deltaTime, BaseCoolDown));
        }
    }
}
