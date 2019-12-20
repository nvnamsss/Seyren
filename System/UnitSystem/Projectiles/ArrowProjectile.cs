using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base2D.System.UnitSystem.Projectiles
{
    public class ArrowProjectile : Projectile
    {
        private float _speedX;
        private float _speedY;
        public ArrowProjectile()
        {
            ProjectileType = ProjectileType.Arrow;
            Speed = 0.1;
        }

        protected override void Start()
        {
            base.Start();
            Body.gravityScale = 1;
            Body.mass = (float)(18.0 / 1000.0);
            Body.mass = 1;
        }
        public override void Move()
        {
            float rad = (float)(Angle * Mathf.Deg2Rad);
            //Vector2 velocity = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * (float)Speed;
            Vector2 velocity = new Vector2(Mathf.Cos(rad) * (float)_speedX, Mathf.Sin(rad) * (float)_speedY);

            Body.velocity = new Vector2(0, 0);
            gameObject.transform.rotation = Quaternion.Euler(0 ,0, Utils.RotationUtil.AngleBetween(Body.transform.position, Body.transform.position + (Vector3)velocity).z);
            //gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y, (float)Angle);
            Angle = Angle - ProjectileArc;
            if (Angle < 0)
            {
                Angle = 0;
            }

            Body.AddForce(velocity, ForceMode2D.Impulse);
        }

        public static ArrowProjectile Create(string name, Vector3 location, Quaternion rotation, Sprite sprite, RuntimeAnimatorController controller, float speed, float arc, float duration = float.MaxValue)
        {
            GameObject go = CreateObject(name, location, rotation, sprite, controller);
            var arrow = go.AddComponent<ArrowProjectile>();
            arrow.Speed = speed;
            arrow.ProjectileArc = arc;
            arrow.TimeExpire = duration;
            arrow.Angle = rotation.eulerAngles.z;
            arrow._speedX = (float)(arrow.Speed * Mathf.Cos((float)(arrow.Angle * Mathf.Deg2Rad)));
            arrow._speedY = (float)(arrow.Speed * Mathf.Sin((float)(arrow.Angle * Mathf.Deg2Rad)));

            return arrow;
        }

        public static ArrowProjectile Create(string name, Vector3 location, float zAngle, Sprite sprite, RuntimeAnimatorController controller, float speed, float arc, float duration = float.MaxValue)
        {
            GameObject go = CreateObject(name, location, Quaternion.Euler(0, 0, zAngle), sprite, controller);
            var arrow = go.AddComponent<ArrowProjectile>();
            arrow.Speed = speed;
            arrow.ProjectileArc = arc;
            arrow.TimeExpire = duration;
            arrow.Angle = zAngle;
            arrow._speedX = (float)(arrow.Speed * Mathf.Cos((float)(arrow.Angle * Mathf.Deg2Rad)));
            arrow._speedY = (float)(arrow.Speed * Mathf.Sin((float)(arrow.Angle * Mathf.Deg2Rad)));


            return arrow;
        }

        public override void Hit()
        {
            base.Hit();
        }
    }
}
