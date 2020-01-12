using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Projectiles;
using Base2D.System.UnitSystem.Units;
using System.Collections;
using UnityEngine;

namespace Base2D.Init.Abilities
{
    public class MagicFlame : Ability
    {
        public static readonly int Id = 0x77707601;
        public Unit unit;
        public MissileProjectile Projectile;
        private Sprite sprite;
        private RuntimeAnimatorController controller;
        private static string magicFlamePath = "Effect/MagicFlame/MagicFlame";
        public MagicFlame(Unit u) : base(u, 0.2f, 1, 1)
        {
            unit = u;
            controller = Resources.Load<RuntimeAnimatorController>(magicFlamePath);
            BaseCoolDown = 1;
            BaseCastingTime = 0.2f;
        }

        public override bool Cast()
        {
            if (!Active ||
                TimeCoolDownLeft > 0 ||
                IsCasting ||
                unit.Action.Type == System.ActionSystem.ActionType.CastAbility ||
                unit.Action.Type == System.ActionSystem.ActionType.Attack)
            {
                return false;
            }


            IsCasting = true;
            unit.Action.Animator.SetBool("Spell", true);
            TimeCastingLeft = 0.2f;
            unit.StartCoroutine(Casting(Time.deltaTime, 0.2f));

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

        public GameObject Create(Vector2 location, Quaternion rotation)
        {
            Vector3 euler = rotation.eulerAngles;
            MissileProjectile missile = MissileProjectile.Create("missile",
                location,
                Quaternion.Euler(0, 0, euler.y == 180 ? 90 : -90),
                sprite,
                controller,
                10,
                2);
            missile.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            missile.Collider.size = new Vector2(1, 2);
            missile.direction = euler.y == 180 ? new Vector2(1, 0) : new Vector2(-1, 0);
            missile.MaxHit = 100000;
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
                    missile.ResetHit();
                }
            };

            return missile.gameObject;
        }

        protected override void DoCastAbility()
        {
            IsCasting = false;
            unit.Action.Animator.SetBool("Spell", false);
            Create(unit.transform.position, unit.transform.rotation);
            TimeCoolDownLeft = BaseCoolDown;
        }

        protected override bool Condition()
        {
            throw new global::System.NotImplementedException();
        }
    }
}
