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
        public Unit Target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
            }
        }

        [SerializeField]
        private Unit _target;
        HomingProjectile()
        {
            Speed = 100;
            _projectileType = ProjectileType.Homing;
        }

        public override void Move()
        {
            if (Target == null)
            {
                return;
            }
            Body.velocity = new Vector2(0, 0);
            Vector3 velocity = (Target.transform.position - transform.position).normalized * (float)Speed;
            Vector3 rotation = Utils.RotationUtils.AngleBetween(transform.position, Target.transform.position);
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

        public static HomingProjectile Create(Unit target, GameObject go)
        {
            HomingProjectile homing = go.AddComponent<HomingProjectile>();
            homing.Target = target;

            return homing;
        }
    }
}
