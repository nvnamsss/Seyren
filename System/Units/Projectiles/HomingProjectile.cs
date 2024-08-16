﻿// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using UnityEngine;

// namespace Seyren.System.Units.Projectiles
// {
//     public class HomingProjectile : Projectile
//     {
//         public Unit Target
//         {
//             get
//             {
//                 return _target;
//             }
//             set
//             {
//                 _target = value;
//             }
//         }

//         [SerializeField]
//         private Unit _target;
//         HomingProjectile()
//         {
//             _projectileType = ProjectileType.Homing;
//         }

//         public override void Move()
//         {
//             if (_target == null)
//             {
//                 return;
//             }
//             Body.velocity = new Vector2(0, 0);
//             Vector3 velocity = (_target.Location - transform.position).normalized * Speed;
//             Vector3 rotation = Utils.RotationUtils.AngleBetween(transform.position, _target.Location);
//             Body.AddForce(velocity, ForceMode2D.Impulse);
//             transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
//         }

//         public static HomingProjectile Create(Unit target, Sprite sprite)
//         {
//             GameObject go = new GameObject("homing");
//             SpriteRenderer render = go.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
//             render.sprite = sprite;

//             go.AddComponent<Rigidbody2D>();
//             var projectile = go.AddComponent<HomingProjectile>();
//             projectile.Target = target;

//             return projectile;
//         }

//         /// <summary>
//         /// Create new Homing base on existed GameObject
//         /// </summary>
//         public static HomingProjectile Create(GameObject go)
//         {
//             GameObject g = Instantiate(go);
//             HomingProjectile homing = g.AddComponent<HomingProjectile>();

//             return homing;
//         }
//         /// <summary>
//         /// Create new Homing base on existed GameObject
//         /// </summary>
//         /// <param name="target">Targeted Unit that Homing will chasing</param>
//         public static HomingProjectile Create(Unit target, GameObject go)
//         {
//             HomingProjectile homing = Create(go);
//             homing.Target = target;

//             return homing;
//         }
//         /// <summary>
//         /// Create new Homing base on existed GameObject then add a collider to created Homing
//         /// </summary>
//         /// <typeparam name="TCollider2D">Collider2D type like BoxCollider2D, CircleCollider2D, etc</typeparam>
//         public static HomingProjectile Create<TCollider2D>(GameObject go) where TCollider2D : Collider2D
//         {
//             GameObject g = Instantiate(go);
//             Rigidbody2D body = g.GetComponent<Rigidbody2D>();
//             TCollider2D collider = g.GetComponent<TCollider2D>();
//             if (body == null) g.AddComponent<Rigidbody2D>();
//             if (collider == null) g.AddComponent<TCollider2D>();
//             HomingProjectile homing = g.AddComponent<HomingProjectile>();

//             return homing;
//         }

//         /// <summary>
//         /// Create new Homing base on existed GameObject then add a collider to created Homing
//         /// </summary>
//         /// <typeparam name="TCollider2D">Collider2D type like BoxCollider2D, CircleCollider2D, etc</typeparam>
//         /// <param name="target">Targeted Unit that Homing will chasing</param>
//         /// <returns></returns>
//         public static HomingProjectile Create<TCollider2D>(Unit target, GameObject go) where TCollider2D : Collider2D
//         {
//             HomingProjectile homing = Create<TCollider2D>(go);
//             homing.Target = target;
//             return homing;
//         }
//     }
// }
