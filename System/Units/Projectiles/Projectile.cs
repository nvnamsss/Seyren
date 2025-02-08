// using System.Collections;
// using System.Collections.Generic;
// using Seyren.System.Damages;
// using Seyren.System.Units;
// using UnityEngine;

// namespace Seyren.System.Units.Projectiles
// {
//     public abstract class Projectile
//     {
//         public delegate void HitExceedHandler(Projectile sender);
//         public delegate void TimeExpiredHandler(Projectile sender);
//         public delegate void OnHitHandler(Projectile sender, GameObject collider);
//         public delegate bool HitConditionHandler<T>(Projectile sender, T hit);
//         public HitConditionHandler<GameObject> HitCondition;
//         public event OnHitHandler OnHit;
//         public event TimeExpiredHandler TimeExpired;
//         public event HitExceedHandler HitExceed;
//         public Collider2D Collider { get; set; }
//         public Rigidbody2D Body { get; set; }
//         public int MaxHit
//         {
//             get => _maxHit;
//             set => _maxHit = value;
//         }
//         public bool IsPenetrate { get; set; }
//         public Unit Owner { get; set; }
//         public Modification Modification { get; set; }

//         public float TimeExpire
//         {
//             get
//             {
//                 return _timeExpired;
//             }
//             set
//             {
//                 _timeExpired = value;
//                 if (_timeExpired < 0)
//                 {
//                     Active = false;
//                     TimeExpired?.Invoke(this);
//                 }
//             }
//         }
//         public bool Active
//         {
//             get
//             {
//                 return _active;
//             }
//             set
//             {
//                 _active = value;
//             }
//         }
//         public Vector2 Forward
//         {
//             get => _forward;
//             set => _forward = value;
//         }
//         public float BaseHitDelay
//         {
//             get
//             {
//                 return _baseHitDelay;
//             }
//             set
//             {
//                 _baseHitDelay = value;
//             }
//         }
//         public float Speed
//         {
//             get
//             {
//                 return _speed;
//             }
//             set
//             {
//                 _speed = value;
//             }
//         }
//         public ProjectileType ProjectileType => _projectileType;

//         protected Animator animator;
//         [SerializeField]
//         protected float _timeExpired;
//         [SerializeField]
//         protected float _baseHitDelay;
//         protected float _hitDelay;
//         [SerializeField]
//         protected float _speed;
//         [SerializeField]
//         protected int _maxHit;
//         [SerializeField]
//         protected int _currentHit;
//         [SerializeField]
//         protected bool _active;
//         [SerializeField]
//         protected Vector2 _forward;
//         protected ProjectileType _projectileType;

//         protected Projectile()
//         {
//             _maxHit = 1;
//             _baseHitDelay = 1;
//             _speed = 1;
//             _hitDelay = 0;
//             _currentHit = 0;
//             _timeExpired = float.MaxValue;
//             _active = true;
//             IsPenetrate = false;
//             HitCondition = (s, obj) => true;
//         }
//         public abstract void Move();

//         public virtual void Hit(GameObject collider)
//         {
//             if (!Active)
//             {
//                 return;
//             }

//             if (_hitDelay > 0)
//             {
//                 return;
//             }

//             if (_currentHit >= MaxHit)
//             {
//                 HitExceed?.Invoke(this);
//                 Destroy(gameObject);
//             }

//             _currentHit = _currentHit + 1;
//             _hitDelay = BaseHitDelay;

//             if (IsPenetrate)
//             {
//                 Collider.isTrigger = true;
//             }

//             OnHit?.Invoke(this, collider);
//         }


//         /// <summary>
//         /// Remove projectile
//         /// </summary>
//         public virtual void Remove()
//         {
//         }

//         public virtual void ResetHit()
//         {
//             _hitDelay = 0;
//             _currentHit += 1;
//         }

//         public void Look(Vector2 direction)
//         {
//             Look(Forward, direction);
//         }

//         public void Look(Vector2 forward, Vector2 direction)
//         {
//             if (!Active)
//             {
//                 return;
//             }

//             float forwardDot = Vector2.Dot(forward, direction);
//             if (forwardDot == 0)
//             {
//                 float angle = Vector2.SignedAngle(forward, direction);
//                 forwardDot = angle > 0 ? -1 : 1;
//             }
//             Vector2 f = forward * forwardDot;
//             Quaternion q1 = Quaternion.FromToRotation(forward, f);
//             Quaternion q2 = Quaternion.FromToRotation(f, direction);
//             transform.rotation = q2 * q1;
//         }

//         public void Look(Vector2 forward, Vector2 upward, Vector2 direction)
//         {

//         }

//         public void Rotate(float xDegree, float yDegree, float zDegree)
//         {
//             Vector3 euler = transform.rotation.eulerAngles;
//             euler.x += xDegree;
//             euler.y += yDegree;
//             euler.z += zDegree;

//             transform.rotation = Quaternion.Euler(euler);
//         }

//         protected virtual void Awake()
//         {
//             Body = GetComponent<Rigidbody2D>();
//             Collider = GetComponent<Collider2D>();
//             animator = GetComponent<Animator>();
//             TimeExpired += (sender) =>
//             {
//                 Destroy(sender.gameObject);
//             };
//         }
//         // Start is called before the first frame update
//         protected virtual void Start()
//         {
//         }

//         // Update is called once per frame
//         protected virtual void FixedUpdate()
//         {
//             _hitDelay -= Time.deltaTime;
//             TimeExpire -= Time.deltaTime;
//             Move();
//         }

//         protected virtual void OnCollisionEnter2D(Collision2D collision)
//         {
//             bool hit = HitCondition(this, collision.gameObject);

//             if (hit)
//             {
//                 Hit(collision.gameObject);
//             }
//         }

//         protected virtual void OnCollisionStay2D(Collision2D collision)
//         {
//             bool hit = HitCondition(this, collision.gameObject);

//             if (hit)
//             {
//                 Hit(collision.gameObject);
//             }
//         }

//         protected virtual void OnTriggerEnter2D(Collider2D collision)
//         {
//             bool hit = HitCondition(this, collision.gameObject);

//             if (hit)
//             {
//                 Hit(collision.gameObject);
//             }
//         }

//         protected virtual void OnTriggerStay2D(Collider2D collision)
//         {
//             bool hit = HitCondition(this, collision.gameObject);

//             if (hit)
//             {
//                 Hit(collision.gameObject);
//             }
//         }

//         protected static GameObject CreateObject(string name, Vector3 location, Quaternion rotation, Sprite sprite, RuntimeAnimatorController controller)
//         {
//             GameObject go = new GameObject(name);
//             SpriteRenderer render = go.AddComponent<SpriteRenderer>();
//             Animator animator = go.AddComponent<Animator>();
//             animator.runtimeAnimatorController = controller;
//             go.transform.position = location;
//             go.transform.rotation = rotation;
//             render.sprite = sprite;
//             go.AddComponent<Rigidbody2D>();
//             go.AddComponent<BoxCollider2D>();
//             return go;
//         }
//     }

// }
