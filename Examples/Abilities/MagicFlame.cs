using Seyren.System.Abilities;
using Seyren.System.Actions;
using Seyren.System.Units.Projectiles;
using Seyren.System.Units;
using System.Collections;
using UnityEngine;

namespace Seyren.Example.Abilities
{
    public class MagicFlame : ActiveAbility, IAction
    {
        public static readonly int Id = 0x77707601;
        private static string magicFlamePath = "Effect/MagicFlame/MagicFlame";
        
        public event ActionHandler ActionStart;
        public event ActionHandler ActionEnd;
        private bool actionRun;
        private GameObject go;
        public ActionConditionHandler RunCondition { get; }

        public ActionType ActionType { get; }

        public MagicFlame(Unit u) : base(u, 0.2f, 1, 1)
        {
            ActionType = ActionType.CastAbility;
            go = Resources.Load<GameObject>(magicFlamePath);
            Casting += (s, e) =>
            {
            };

            CastCompleted += (s) =>
            {
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
                if (action == this || action.ActionType == ActionType.CastAbility)
                {
                    return false;
                }   
                return true;
            };
        }

        public GameObject Create(Vector2 location, Quaternion rotation)
        {
            Vector2 direction = Caster.transform.rotation * Caster.Forward;
            MissileProjectile missile = MissileProjectile.Create<CapsuleCollider2D>(direction, go);
            missile.Speed = 10;
            missile.Forward = new Vector2(0, -1);
            missile.Collider.isTrigger = true;
            missile.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            missile.MaxHit = 100000;
            missile.Owner = Caster;
            missile.transform.position = location;
            missile.HitCondition = (s, obj) =>
            {
                Unit u = obj.GetComponent<Unit>();
                return u != null && Caster.IsEnemy(u);
            };

            missile.OnHit += (s, e) =>
            {
                Unit u = e.GetComponent<Unit>();
                u.Damage(missile.Owner, System.Damages.DamageType.Physical);
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
