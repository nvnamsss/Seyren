using System.Collections;
using System.Collections.Generic;
using Base2D.System.DamageSystem;
using Base2D.System.UnitSystem.EventData;
using Base2D.System.UnitSystem.Units;
using UnityEngine;

namespace Base2D.System.UnitSystem.Projectiles
{
    public abstract partial class Projectile : MonoBehaviour, IAttribute
    {
        protected Projectile()
        {
            _maxHit = 1;
            _baseHitDelay = 1;
            _speed = 1;
            _hitDelay = 0;
            _currentHit = 0;
            _timeExpired = float.MaxValue;
            _active = true;
            IsPenetrate = false;
        }
        public abstract void Move();

        public virtual void Hit(GameObject collider)
        {
            if (!Active)
            {
                return;
            }

            if (_hitDelay > 0)
            {
                return;
            }

            if (_currentHit >= MaxHit)
            {
                HitExceed?.Invoke(this);
                Destroy(gameObject);
            }

            _currentHit = _currentHit + 1;
            _hitDelay = BaseHitDelay;

            if (IsPenetrate)
            {
                Collider.isTrigger = true;
            }

            OnHit?.Invoke(this, collider);
        }


        /// <summary>
        /// Remove projectile
        /// </summary>
        public virtual void Remove()
        {
        }

        public virtual void ResetHit()
        {
            _hitDelay = 0;
            _currentHit += 1;
        }

        public void Look(Vector2 direction)
        {
            float dot = Vector2.Dot(BaseLook, direction);

            Vector3 look = Vector3.Cross(BaseLook, direction);
            if (look == Vector3.zero)
            {

            }   
            float w = Mathf.Sqrt(Mathf.Pow(BaseLook.magnitude, 2) * Mathf.Pow(direction.magnitude, 2)) + dot;
            Quaternion quaternion = new Quaternion(look.x, look.y, look.z, w);
            transform.rotation = quaternion.normalized;
            transform.rotation = Quaternion.FromToRotation(BaseLook, direction);
        }

        protected virtual void Awake()
        {
            Body = GetComponent<Rigidbody2D>();
            Collider = GetComponent<Collider2D>();
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
            ConditionEventArgs<GameObject> e = new ConditionEventArgs<GameObject>(collision.gameObject, true);
            Condition?.Invoke(this, e);

            if (e.Match)
            {
                Hit(collision.gameObject);
            }
        }

        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            ConditionEventArgs<GameObject> e = new ConditionEventArgs<GameObject>(collision.gameObject, true);
            Condition?.Invoke(this, e);

            if (e.Match)
            {
                Hit(collision.gameObject);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            ConditionEventArgs<GameObject> e = new ConditionEventArgs<GameObject>(collision.gameObject, true);
            Condition?.Invoke(this, e);

            if (e.Match)
            {
                Hit(collision.gameObject);
            }
        }

        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            ConditionEventArgs<GameObject> e = new ConditionEventArgs<GameObject>(collision.gameObject, true);
            Condition?.Invoke(this, e);

            if (e.Match)
            {
                Hit(collision.gameObject);
            }
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
