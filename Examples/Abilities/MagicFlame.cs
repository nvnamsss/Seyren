using Seyren.System.Abilities;
using Seyren.System.Actions;
using Seyren.System.Units.Projectiles;
using Seyren.System.Units;
using System.Collections;
using UnityEngine;
using Seyren.System.Generics;

namespace Seyren.Examples.Abilities
{
    public class MagicFlame : ActiveAbility
    {
        public static readonly int Id = 0x77707601;
        private static string magicFlamePath = "Effect/MagicFlame/MagicFlame";
        
        private bool actionRun;
        private GameObject go;
        public ActionConditionHandler RunCondition { get; }


        public MagicFlame(Unit u) : base( 0.2f, 1, 1)
        {

        }

        public GameObject Create(Vector2 location, Quaternion rotation)
        {
            // Vector2 direction = Caster.transform.rotation * Caster.Forward;
            // MissileProjectile missile = MissileProjectile.Create<CapsuleCollider2D>(direction, go);
            // missile.Speed = 10;
            // missile.Forward = new Vector2(0, -1);
            // missile.Collider.isTrigger = true;
            // missile.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            // missile.MaxHit = 100000;
            // missile.Owner = Caster;
            // missile.transform.position = location;
            // missile.HitCondition = (s, obj) =>
            // {
            //     Unit u = obj.GetComponent<Unit>();
            //     return u != null && Caster.IsEnemy(u);
            // };

            // missile.OnHit += (s, e) =>
            // {
            //     Unit u = e.GetComponent<Unit>();
            //     u.Damage(missile.Owner, System.Damages.DamageType.Physical);
            // };

            return null;
        }
        protected override void onCast(Unit by)
        {
            throw new global::System.NotImplementedException();
        }

        protected override void onCast(Unit by, Unit target)
        {
            throw new global::System.NotImplementedException();
        }

        protected override void onCast(Unit by, Vector3 target)
        {
            throw new global::System.NotImplementedException();
        }

        protected override void DoCastAbility()
        {
            throw new global::System.NotImplementedException();
        }

        protected override Error Condition(Unit by)
        {
            throw new global::System.NotImplementedException();
        }

        protected override Error Condition(Unit by, Unit target)
        {
            throw new global::System.NotImplementedException();
        }

        protected override Error Condition(Unit by, Vector3 target)
        {
            throw new global::System.NotImplementedException();
        }

        public override long CastTime(Unit unit)
        {
            throw new global::System.NotImplementedException();
        }

        public override IAction Action(Unit by)
        {
            throw new global::System.NotImplementedException();
        }

        public override IAction Action(Unit by, Unit target)
        {
            throw new global::System.NotImplementedException();
        }

        public override IAction Action(Unit by, Vector3 target)
        {
            throw new global::System.NotImplementedException();
        }
    }
}
