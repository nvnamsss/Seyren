// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using UnityEngine;

// namespace Seyren.System.Units.Projectiles
// {
//     public class ArrowProjectile : Projectile
//     {
//         public Vector2 Arc
//         {
//             get => _arc;
//             set => _arc = value;
//         }

//         public Vector2 Direction;
//         [SerializeField]
//         private Vector2 _arc;
//         ArrowProjectile()
//         {
//             _projectileType = ProjectileType.Arrow;
//         }

//         protected override void Start()
//         {
//             base.Start();
//             Body.gravityScale = 1;
//             Body.mass = (float)(18.0 / 1000.0);
//             Body.mass = 1;
//         }
//         public override void Move()
//         {
//             Direction = Direction + Arc;
//             Vector2 velocity = Direction * Speed;
            
//             Body.velocity = new Vector2(0, 0);      
//             Body.AddForce(velocity, ForceMode2D.Impulse);
//             Look(velocity);
//         }
        

//         public override void Hit(GameObject collider)
//         {
//             base.Hit(collider);
//         }

//         /// <summary>
//         /// Create new Arrow base on existed GameObject
//         /// </summary>
//         public static ArrowProjectile Create(GameObject go)
//         {
//             GameObject g = Instantiate(go);
//             ArrowProjectile arrow = g.AddComponent<ArrowProjectile>();
//             return arrow;
//         }

//         /// <summary>
//         /// Create new Arrow base on existed GameObject
//         /// </summary>
//         /// <param name="direction">Fly direction of arrow</param>
//         /// <param name="arc">Turn speed of arrow, higher arc cause arrow change direction faster</param>
//         public static ArrowProjectile Create(Vector2 direction, Vector2 arc, GameObject go)
//         {
//             ArrowProjectile arrow = Create(go);
//             arrow.Direction = direction;
//             arrow.Arc = arc;

//             return arrow;
//         }
//         /// <summary>
//         /// Create new Arrow base on existed GameObject then add a collider to created arrow
//         /// </summary>
//         /// <typeparam name="TCollider2D">Collider2D type like BoxCollider2D, CircleCollider2D, etc</typeparam>
//         public static ArrowProjectile Create<TCollider2D>(GameObject go) where TCollider2D : Collider2D
//         {
//             GameObject g = Instantiate(go);
//             Rigidbody2D body = g.GetComponent<Rigidbody2D>();
//             TCollider2D collider = g.GetComponent<TCollider2D>();
//             if (body == null) g.AddComponent<Rigidbody2D>();
//             if (collider == null) g.AddComponent<TCollider2D>();
//             ArrowProjectile arrow = g.AddComponent<ArrowProjectile>();
//             return arrow;
//         }
//         /// <summary>
//         /// Create new Arrow base on existed GameObject then add a collider to created arrow
//         /// </summary>
//         /// <typeparam name="TCollider2D">Collider2D type like BoxCollider2D, CircleCollider2D, etc</typeparam>
//         /// <param name="direction">Fly direction of arrow</param>
//         /// <param name="arc">Turn speed of arrow, higher arc cause arrow change direction faster</param>
//         /// <returns></returns>
//         public static ArrowProjectile Create<TCollider2D>(Vector2 direction, Vector2 arc, GameObject go) where TCollider2D : Collider2D
//         {
//             ArrowProjectile arrow = Create<TCollider2D>(go);
//             arrow.Direction = direction;
//             arrow.Arc = arc;
//             return arrow;
//         }
//     }
// }
