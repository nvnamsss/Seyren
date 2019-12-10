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
        }
        public override void Move()
        {
            float rad = (float)(Angle * Mathf.Deg2Rad);
            Vector2 velocity = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)) * (float)Speed;
            Debug.Log("Move");
            Angle = Angle + ProjectileArc;
            Speed = Speed * 0.5;
            Body.AddForce(velocity, ForceMode2D.Impulse);
        }

        public override void Hit()
        {
            base.Hit();
        }
    }
}
