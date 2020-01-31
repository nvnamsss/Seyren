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
        public delegate void HitExceedHandler(Projectile sender);
        public delegate void TimeExpiredHandler(Projectile sender);
        public delegate void OnHitHandler(Projectile sender, GameObject collider);
        public delegate void ConditionHandler(Projectile sender, ConditionEventArgs<GameObject> e);
        public ConditionHandler Condition;
        public event OnHitHandler OnHit;
        public event TimeExpiredHandler TimeExpired;
        public event HitExceedHandler HitExceed;
        public Collider2D Collider { get; set; }
        public Rigidbody2D Body { get; set; }
        public int MaxHit
        {
            get => _maxHit;
            set => _maxHit = value;
        }
        public bool IsPenetrate { get; set; }
        public Unit Owner { get; set; }
        public Attribute Attribute { get; set; }
        public ModificationInfos Modification { get; set; }

        public float TimeExpire
        {
            get
            {
                return _timeExpired;
            }
            set
            {
                _timeExpired = value;
                if (_timeExpired < 0)
                {
                    Active = false;
                    TimeExpired?.Invoke(this);
                }
            }
        }
        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
            }
        }
        public Vector2 Forward
        {
            get => _forward;
            set => _forward = value;
        }
        public float BaseHitDelay
        {
            get
            {
                return _baseHitDelay;
            }
            set
            {
                _baseHitDelay = value;
            }
        }
        public float Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }
        public ProjectileType ProjectileType => _projectileType;

        protected Animator animator;
        [SerializeField]
        protected float _timeExpired;
        [SerializeField]
        protected float _baseHitDelay;
        protected float _hitDelay;
        [SerializeField]
        protected float _speed;
        [SerializeField]
        protected int _maxHit;
        [SerializeField]
        protected int _currentHit;
        [SerializeField]
        protected bool _active;
        [SerializeField]
        protected Vector2 _forward;
        protected ProjectileType _projectileType;
    }
}
