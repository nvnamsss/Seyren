﻿using Base2D.System.UnitSystem.Units;
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
        public HomingProjectile()
        {
            Speed = 100;
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
            Body.velocity = new Vector2(0, 0);
            Vector3 velocity = (Target.transform.position - transform.position).normalized * (float)Speed;
            Vector3 rotation = Utils.RotationUtil.AngleBetween(transform.position, Target.transform.position);

            Body.AddForce(velocity, ForceMode2D.Impulse);
            transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        }

        public static HomingProjectile Create(Unit target, Sprite sprite)
        {
            GameObject go = new GameObject("homing");
            SpriteRenderer render = go.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            render.sprite = sprite;

            go.AddComponent<Rigidbody2D>();
            var projectile = go.AddComponent<HomingProjectile>();
            projectile.Target = target;

            return projectile;
        }
    }
}