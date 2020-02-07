using Base2D.System.AbilitySystem;
using Base2D.System.ActionSystem;
using Base2D.System.UnitSystem.Projectiles;
using Base2D.System.UnitSystem.Units;
using System.Collections;
using UnityEngine;

namespace Base2D.Init.Abilities
{
    public class MagicFlame : ActiveAbility, IAction
    {
        public static readonly int Id = 0x77707601;
        public MissileProjectile Projectile;
        private Sprite sprite;
        private RuntimeAnimatorController controller;
        private static string magicFlamePath = "Effect/MagicFlame/MagicFlame";

        public event ActionHandler StartAction;
        public event ActionHandler EndAction;
        private bool actionRun;
        public MagicFlame(Unit u) : base(u, 0.2f, 1, 1)
        {
            controller = Resources.Load<RuntimeAnimatorController>(magicFlamePath);
            BaseCoolDown = 1;
            BaseCastingTime = 0.2f;
           
            Casting += (s, e) =>
            {
            };

            Casted += (s) =>
            {
            };

            StartAction += (s) =>
            {
                actionRun = true;
                Caster.Action.Animator.SetBool("Spell", true);
            };

            EndAction += (s) =>
            {
                actionRun = false;
                Caster.Action.Animator.SetBool("Spell", false);
            };
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
            //missile.Collider.size = new Vector2(1, 2);
            missile.Direction = euler.y == 180 ? new Vector2(1, 0) : new Vector2(-1, 0);
            missile.MaxHit = 100000;
            missile.Owner = Caster;
            missile.HitCondition = (s, obj) =>
            {
                Unit u = obj.GetComponent<Unit>();
                return u != null && Caster.IsEnemy(u);
            };

            missile.OnHit += (s, e) =>
            {
                Unit u = e.GetComponent<Unit>();
                u.Damage(missile.Owner, System.DamageSystem.DamageType.Physical);
            };

            return missile.gameObject;
        }

        protected override void DoCastAbility()
        {
            Create(Caster.transform.position, Caster.transform.rotation);
            if (actionRun)
                Revoke();
        }

        protected override bool Condition()
        {
            return !(!Active ||
                TimeCoolDownLeft > 0 ||
                IsCasting);
        }

        public void Invoke()
        {
            StartAction?.Invoke(this);
            bool cast = Cast();
            if (!cast)
            {
                Revoke();
            }
        }

        public void Revoke()
        {
            EndAction?.Invoke(this);
            Caster.StopCoroutine(castCoroutine);
        }
    }
}
