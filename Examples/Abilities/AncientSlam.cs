using Seyren.System.Abilities;
using Seyren.System.Actions;
using Seyren.System.Generics;
using Seyren.System.Units;
using Seyren.System.Units.Projectiles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.Examples.Abilities
{
    public class AncientSlam : ActiveAbility
    {
        public static readonly int Id = 0x65678301;
        private Sprite sprite;
        private RuntimeAnimatorController controller;
        private Dictionary<Unit, int> hitList;
        private static string ancientEnergyPath = "Effect/AncientEnergy/AncientEnergy";

        public ActionConditionHandler RunCondition { get; }

        public ActionType ActionType => ActionType.CastAbility;

        private GameObject go;
        private bool actionRun;

        public AncientSlam(Unit u) : base( 2, 10, 1)
        {
         
        }

        public GameObject Create(Vector2 location, Quaternion rotation)
        {
            return null;
        }

        public void Create()
        {
            // float angle = 180;
            // for (int loop = 0; loop < 7; loop++)
            // {
            //     float rad = (angle * Mathf.Deg2Rad);
            //     Vector2 location = Caster.transform.position;
            //     location.y -= 1;
            //     Quaternion rotation = Caster.transform.rotation;
            //     Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            //     MissileProjectile missile = MissileProjectile.Create("Ancient Slam - Energy",
            //         location,
            //         rotation,
            //         sprite,
            //         controller,
            //         10,
            //         2);
            //     missile.HitCondition = HitCondition;
            //     missile.BaseHitDelay = 0;
            //     //missile.Collider.size = new Vector2(0.86f, 0.86f);
            //     missile.Direction = direction;
            //     missile.transform.localScale = new Vector3(3, 3, 1);
            //     missile.MaxHit = 100000;
            //     missile.Owner = Caster;
            //     missile.OnHit += (sender, e) =>
            //     {
            //         Unit u = e.GetComponent<Unit>();
            //         hitList.Add(u, 0);
            //         u.Damage(sender.Owner, System.Damages.DamageType.Physical);
            //     };
            //     angle = angle - 30;
            // }
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
            throw new NotImplementedException();
        }

        protected override void onCast(Unit by)
        {
            throw new NotImplementedException();
        }

        protected override void onCast(Unit by, Unit target)
        {
            throw new NotImplementedException();
        }

        protected override void onCast(Unit by, Vector3 target)
        {
            // pick every unit in range then apply a function
            throw new NotImplementedException();
        }


        protected override Error Condition(Unit by)
        {
            throw new NotImplementedException();
        }

        protected override Error Condition(Unit by, Unit target)
        {
            throw new NotImplementedException();
        }

        protected override Error Condition(Unit by, Vector3 target)
        {
            throw new NotImplementedException();
        }
    }
}
