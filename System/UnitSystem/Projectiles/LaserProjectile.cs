using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Crom.System.UnitSystem.Projectiles
{
    public class LaserProjectile : Projectile
    {
        public LaserProjectile()
        {
            MaxHit = int.MaxValue;

        }

        protected override void Start()
        {
            base.Start();
        }

        public override void Hit()
        {
            base.Hit();
        }

        public override void Move()
        {
            float rad = (float)(Angle * Mathf.Deg2Rad);
            Vector2 velocity = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)) * (float)Speed;
            Body.AddForce(velocity, ForceMode2D.Impulse);
        }
    }
}
