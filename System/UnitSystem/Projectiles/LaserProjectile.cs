using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base2D.System.UnitSystem.Projectiles
{
    public class LaserProjectile : Projectile
    {
        public Vector2 direction;
        public float distance;
        LaserProjectile()
        {
            Speed = 3 * Mathf.Pow(10, 8);
            MaxHit = int.MaxValue;
            _projectileType = ProjectileType.Laser;
        }

        protected override void Start()
        {
            base.Start();
        }

        public override void Hit(GameObject collider)
        {
            base.Hit(collider);
        }

        public override void Move()
        {
            Vector2 velocity = direction * (float)Speed;
            Body.AddForce(velocity, ForceMode2D.Impulse);
        }
    }
}
