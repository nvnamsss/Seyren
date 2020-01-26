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

        public static MissileProjectile Create(GameObject go)
        {
            MissileProjectile missile = go.AddComponent<MissileProjectile>();

            return missile;
        }
    }
}
