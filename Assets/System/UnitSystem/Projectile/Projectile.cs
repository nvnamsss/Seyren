using System.Collections;
using System.Collections.Generic;
using Crom.System.DamageSystem;
using UnityEngine;

namespace Crom.System.UnitSystem.Projectile
{
    public class Projectile : MonoBehaviour, IProjectile
    {
        public delegate void OnHitHandler(Projectile sender);
        public event OnHitHandler OnHit;
        public BoxCollider2D Collider { get; set; }
        public Rigidbody2D Body { get; set; }
        public int MaxHit { get; set; }
        public bool IsPenetrate { get; set; }
        public IUnit Owner { get; set; }
        public Attribute Attribute { get; set; }
        public ModificationInfos Modification { get; set; }
        public double ProjectileArc { get; set; }
        public double Angle { get; set; }
        public double TimeExpired { get; set; }
        public double HitDelay { get; set; }
        public double Speed { get; set; }
        public ProjectileType Type { get; set; }

        private double _hitDelay;
        private int _hit;
        public Projectile()
        {
            HitDelay = 10;
            Speed = 0.01f;
            Angle = 90;
            IsPenetrate = false;
            Type = ProjectileType.Arrow;
            _hitDelay = 0;
        }
        public void Hit()
        {
            Debug.Log("[Projectile] - Hit");
            if (_hitDelay > 0)
            {
                return;
            }

            if (_hit >= MaxHit)
            {
                //Destroy(gameObject);               
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

        public void Move()
        {
            float rad = (float)(Angle * Mathf.Deg2Rad);
            Vector2 velocity = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)) * (float)Speed;
            switch (Type)
            {
                case ProjectileType.None:
                    break;
                case ProjectileType.Arrow:
                    Angle = Angle + ProjectileArc;
                    Speed = Speed * 0.5;
                    Body.AddForce(velocity, ForceMode2D.Impulse);
                    break;
                case ProjectileType.Missile:
                    Body.velocity = Vector2.zero;
                    Body.angularVelocity = 0;
                    Body.AddForce(velocity, ForceMode2D.Impulse);
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
        void Start()
        {
            Body = GetComponent<Rigidbody2D>();
            Body.mass = 1f;
            Body.gravityScale = 0;
            Collider = GetComponent<BoxCollider2D>();
            Collider.size = new Vector2(1, 1);
            Collider.isTrigger = false;
            switch (Type)
            {
                case ProjectileType.None:
                    break;
                case ProjectileType.Arrow:
                    Body.gravityScale = 1;
                    Body.mass = (float)(18.0 / 1000.0);
                    Speed = 0.1;
                    break;
                case ProjectileType.Missile:
                    Body.gravityScale = 0;
                    break;
                case ProjectileType.Laser:
                    Body.gravityScale = 0;
                    Speed = 300000000;
                    break;
                case ProjectileType.Custom:
                    break;
                default:
                    break;
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            _hitDelay -= Time.deltaTime;
            Move();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            Hit();
        }

        void OnCollisionStay2D(Collision2D collision)
        {
            Hit();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            Hit();
        }

        void OnTriggerStay2D(Collider2D collision)
        {
            Hit();
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log("[Projectile] - Exit");

        }
    }

}
