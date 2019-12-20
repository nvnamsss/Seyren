using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base2D.System.UnitSystem.Projectiles
{
    public class MissileProjectile : Projectile
    {
        public MissileProjectile()
        {
            ProjectileType = ProjectileType.Missile;
            Body.gravityScale = 0;
        }

        protected override void Start()
        {
            base.Start();
        }

        public override void Move()
        {
            float rad = (float)(Angle * Mathf.Deg2Rad);
            Vector2 velocity = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)) * (float)Speed;
            Body.velocity = Vector2.zero;
            Body.angularVelocity = 0;
            Body.AddForce(velocity, ForceMode2D.Impulse);
        }

        public override void Hit(GameObject collider)
        {
            base.Hit(collider);
        }
    }
}
