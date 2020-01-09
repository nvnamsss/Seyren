using System.Collections;
using System.Collections.Generic;
using Base2D.System.DamageSystem;
using Base2D.System.UnitSystem.Units;
using UnityEngine;

namespace Base2D.System.UnitSystem.Projectiles
{
    public partial class Projectile : MonoBehaviour, IAttribute
    {
        protected Projectile()
        {
            MaxHit = 1;
            HitDelay = 1;
            Speed = 0.01f;
            Angle = 90;
            _hitDelay = 0;
            _hit = 0;
            TimeExpire = float.MaxValue;
            IsPenetrate = false;
            Active = true;
            ProjectileType = ProjectileType.None;
        }
        public virtual void Hit(GameObject collider)
        {
            if (!Active)
            {
                return;
            }

            if (animator != null && animator.isInitialized)
            {
            }

            if (_hitDelay > 0)
            {
                return;
            }

            if (_hit >= MaxHit)
            {
                HitExceed?.Invoke(this);
                Destroy(gameObject);
            }

            _hit = _hit + 1;
            _hitDelay = HitDelay;

            if (!IsPenetrate)
            {
                Collider.isTrigger = true;
            }
            Debug.Log("God");

            OnHit?.Invoke(this, collider);
        }

        public virtual void Move()
        {
            animator?.SetBool("move", true);
            float rad = (float)(Angle * Mathf.Deg2Rad);
            Vector2 velocity = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)) * (float)Speed;
        }

        /// <summary>
        /// Remove projectile
        /// </summary>
        public virtual void Remove()
        {

        }

        protected virtual void Awake()
        {
            Body = GetComponent<Rigidbody2D>();
            Collider = GetComponent<BoxCollider2D>();
            animator = GetComponent<Animator>();
            TimeExpired += (sender) =>
            {
                Destroy(sender.gameObject);
            };
        }
        // Start is called before the first frame update
        protected virtual void Start()
        {
        }

        // Update is called once per frame
        protected virtual void FixedUpdate()
        {
            _hitDelay -= Time.deltaTime;
            TimeExpire -= Time.deltaTime;
            Move();
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            Hit(collision.gameObject);
        }

        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            Hit(collision.gameObject);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            Hit(collision.gameObject);
        }

        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            Hit(collision.gameObject);
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log("[Projectile] - Trigger Exit");

        }

        protected static GameObject CreateObject(string name, Vector3 location, Quaternion rotation, Sprite sprite, RuntimeAnimatorController controller)
        {
            GameObject go = new GameObject(name);
            SpriteRenderer render = go.AddComponent<SpriteRenderer>();
            Animator animator = go.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            go.transform.position = location;
            go.transform.rotation = rotation;
            render.sprite = sprite;
            go.AddComponent<Rigidbody2D>();
            go.AddComponent<BoxCollider2D>();
            return go;
        }
    }

}
