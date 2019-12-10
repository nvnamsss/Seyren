using Base2D.System.UnitSystem.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base2D.System.UnitSystem.Projectiles
{
    public class HomingProjectile : Projectile
    {
        public Unit Target;
        public HomingProjectile()
        {
            ProjectileType = ProjectileType.Homing;
        }

        public HomingProjectile(Unit target) : base()
        {
            Target = target;
        }

        public override void Move()
        {
            if (Target == null)
            {
                return;
            }

            Vector3 velocity = (Target.transform.position - transform.position) ;
            Vector3 rotation = Base2D.Utils.RotationUtil.AngleBetween(transform.position, Target.transform.position);

            Body.AddForce(velocity, ForceMode2D.Impulse);
            transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        }
    }
}
