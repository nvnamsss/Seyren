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
        public Vector2 direction;
        MissileProjectile()
        {
            _projectileType = ProjectileType.Missile;
        }

        protected override void Start()
        {
            base.Start();
            Body.gravityScale = 0;
        }

        public override void Move()
        {
            Vector2 velocity = direction * (float)Speed * Time.deltaTime;
            Body.AddForce(velocity, ForceMode2D.Impulse);
        }

        public override void Hit(GameObject collider)
        {
            base.Hit(collider);
        }

        public static MissileProjectile Create(string name, Vector2 location, Quaternion rotation, Sprite sprite, RuntimeAnimatorController controller, float speed, float duration = float.MaxValue)
        {
            GameObject go = CreateObject(name, location, rotation, sprite, controller);
            MissileProjectile missile = go.AddComponent<MissileProjectile>();
            missile.Speed = speed;
            missile.TimeExpire = duration;
            return missile;
        }

        /// <summary>
        /// Create new Missile base on existed GameObject
        /// </summary>
        public static MissileProjectile Create(GameObject go)
        {
            GameObject g = Instantiate(go);
            MissileProjectile missile = g.AddComponent<MissileProjectile>();

            return missile;
        }

        /// <summary>
        /// Create new Missile base on existed GameObject then add a collider to created Missile
        /// </summary>
        /// <typeparam name="TCollider2D">Collider2D type like BoxCollider2D, CircleCollider2D, etc</typeparam>
        public static MissileProjectile Create<TCollider2D>(GameObject go) where TCollider2D : Collider2D
        {
            GameObject g = Instantiate(go);
            Rigidbody2D body = g.GetComponent<Rigidbody2D>();
            if (body == null) g.AddComponent<Rigidbody2D>();
            g.AddComponent<TCollider2D>();
            MissileProjectile missile = g.AddComponent<MissileProjectile>();
            //Rigidbody2D body = g.GetComponent<Rigidbody2D>();
            //Collider2D collider = g.GetComponent<Collider2D>();

            return missile;
        }
    }
}
