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

        /// <summary>
        /// Create new Laser base on existed GameObject
        /// </summary>
        public static LaserProjectile Create(GameObject go)
        {
            GameObject g = Instantiate(go);
            LaserProjectile laser = g.AddComponent<LaserProjectile>();

            return laser;
        }

        /// <summary>
        /// Create new Laser base on existed GameObject then add a collider to created Laser
        /// </summary>
        /// <typeparam name="TCollider2D">Collider2D type like BoxCollider2D, CircleCollider2D, etc</typeparam>
        public static LaserProjectile Create<TCollider2D>(GameObject go) where TCollider2D : Collider2D
        {
            GameObject g = Instantiate(go);
            Rigidbody2D body = g.GetComponent<Rigidbody2D>();
            TCollider2D collider = g.GetComponent<TCollider2D>();
            if (body == null) g.AddComponent<Rigidbody2D>();
            if (collider == null) g.AddComponent<TCollider2D>();
            LaserProjectile laser = g.AddComponent<LaserProjectile>();
            return laser;
        }
    }
}
