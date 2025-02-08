// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using UnityEngine;

// namespace Seyren.System.Units.Projectiles
// {
//     public class LaserProjectile : Projectile
//     {
//         public Vector2 Direction;
//         public float Distance;
//         LaserProjectile()
//         {
//             Speed = 3 * Mathf.Pow(10, 8);
//             MaxHit = int.MaxValue;
//             _projectileType = ProjectileType.Laser;
//         }

//         protected override void Start()
//         {
//             base.Start();
//         }

//         public override void Hit(GameObject collider)
//         {
//             base.Hit(collider);
//         }

//         public override void Move()
//         {
//             Vector2 velocity = Direction * (float)Speed;
//             Body.AddForce(velocity, ForceMode2D.Impulse);
//         }

//         /// <summary>
//         /// Create new Laser base on existed GameObject
//         /// </summary>
//         public static LaserProjectile Create(GameObject go)
//         {
//             GameObject g = Instantiate(go);
//             LaserProjectile laser = g.AddComponent<LaserProjectile>();

//             return laser;
//         }

//         /// <summary>
//         /// Create new Laser base on existed GameObject
//         /// </summary>
//         /// <param name="direction">Fly direction of arrow</param>
//         /// <param name="distance">Max distance laser can go</param>
//         public static LaserProjectile Create(Vector2 direction, float distance, GameObject go)
//         {
//             LaserProjectile laser = Create(go);
//             laser.Direction = direction;
//             laser.Distance = distance;

//             return laser;
//         }
//         /// <summary>
//         /// Create new Laser base on existed GameObject then add a collider to created Laser
//         /// </summary>
//         /// <typeparam name="TCollider2D">Collider2D type like BoxCollider2D, CircleCollider2D, etc</typeparam>
//         public static LaserProjectile Create<TCollider2D>(GameObject go) where TCollider2D : Collider2D
//         {
//             GameObject g = Instantiate(go);
//             Rigidbody2D body = g.GetComponent<Rigidbody2D>();
//             TCollider2D collider = g.GetComponent<TCollider2D>();
//             if (body == null) g.AddComponent<Rigidbody2D>();
//             if (collider == null) g.AddComponent<TCollider2D>();
//             LaserProjectile laser = g.AddComponent<LaserProjectile>();
//             return laser;
//         }

//         /// <summary>
//         /// Create new Laser base on existed GameObject then add a collider to created Laser
//         /// </summary>
//         /// <typeparam name="TCollider2D"></typeparam>
//         /// <param name="direction">Fly direction of arrow</param>
//         /// <param name="distance">Max distance laser can go</param>
//         public static LaserProjectile Create<TCollider2D>(Vector2 direction, float distance, GameObject go) where TCollider2D : Collider2D
//         {
//             LaserProjectile laser = Create<TCollider2D>(go);
//             laser.Direction = direction;
//             laser.Distance = distance;

//             return laser;
//         }
//     }
// }
