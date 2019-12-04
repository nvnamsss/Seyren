using System.Collections;
using System.Collections.Generic;
using Crom.System.DamageSystem;
using Crom.System.UnitSystem.Units;
using UnityEngine;

namespace Crom.System.UnitSystem.Projectiles
{
    public class Projectile : MonoBehaviour, IAttribute
    {
        public delegate void OnHitHandler(Projectile sender);
        public event OnHitHandler OnHit;
        public BoxCollider2D Collider { get; set; }
        public Rigidbody2D Body { get; set; }
        public int MaxHit { get; set; }
        public bool IsPenetrate { get; set; }
        public Unit Owner { get; set; }
        public Unit Target { get; set; }
        public Attribute Attribute { get; set; }
        public ModificationInfos Modification { get; set; }
        public double ProjectileArc { get; set; }
        public double Angle { get; set; }
        public double TimeExpired { get; set; }
        public double HitDelay;
        public double Speed;
        public ProjectileType ProjectileType { get; set; }

        private double _hitDelay;
        private int _hit;
        public Projectile()
        {
            HitDelay = 10;
            Speed = 0.01f;
            Angle = 90;
            IsPenetrate = false;
            _hitDelay = 0;
            ProjectileType = ProjectileType.None;
        }
        public virtual void Hit()
        {
            if (_hitDelay > 0)
            {
                return;
            }

            if (_hit >= MaxHit)
            {
                Destroy(gameObject);               
            }
            //do something
            _hit = _hit + 1;
            _hitDelay = HitDelay;

            if (!IsPenetrate)
            {
                Collider.isTrigger = true;
            }
            else
            {
                //Destroy(gameObject);
            }

            OnHit?.Invoke(this);
        }

        public virtual void Move()
        {
            float rad = (float)(Angle * Mathf.Deg2Rad);
            Vector2 velocity = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)) * (float)Speed;
            switch (Type)
            {
                case ProjectileType.None:
                    break;
                
                case ProjectileType.Missile:
                    
                    break;
                case ProjectileType.Laser:
                    Body.AddForce(velocity, ForceMode2D.Impulse);
                    break;
                case ProjectileType.Custom:
                    break;
                default:
                    break;
            }
            
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            Body = GetComponent<Rigidbody2D>();

            Collider = GetComponent<BoxCollider2D>();

            //Body.mass = 1f;
            //Body.gravityScale = 0;
            //switch (Type)
            //{
            //    case ProjectileType.None:
            //        break;
            //    case ProjectileType.Arrow:
                    
            //        break;
            //    case ProjectileType.Missile:
            //        Body.gravityScale = 0;
            //        break;
            //    case ProjectileType.Laser:
            //        Body.gravityScale = 0;
            //        Speed = 300000000;
            //        break;
            //    case ProjectileType.Custom:
            //        break;
            //    default:
            //        break;
            //}
        }

        // Update is called once per frame
        protected virtual void FixedUpdate()
        {
            _hitDelay -= Time.deltaTime;
            Move();
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            Hit();
        }

        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            Hit();
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            Hit();
        }

        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            Hit();
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log("[Projectile] - Exit");

        }
    }

}
