using System.Collections;
using System.Collections.Generic;
using Base2D.System.DamageSystem;
using Base2D.System.UnitSystem.Units;
using UnityEngine;

namespace Base2D.System.UnitSystem.Projectiles
{
    public partial class Projectile : MonoBehaviour, IAttribute
    {
        public delegate void TimeExpiredHandler(Projectile sender);
        public delegate void OnHitHandler(Projectile sender);
        public event OnHitHandler OnHit;
        public event TimeExpiredHandler TimeExpired;
        public BoxCollider2D Collider { get; set; }
        public Rigidbody2D Body { get; set; }
        public int MaxHit { get; set; }
        public bool IsPenetrate { get; set; }
        public Unit Owner { get; set; }
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
        public Attribute Attribute { get; set; }
        public ModificationInfos Modification { get; set; }
        public double ProjectileArc { get; set; }
        public double Angle { get; set; }
        public double TimeExpire
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
                    animator.SetBool("expired", true);
                    TimeExpired?.Invoke(this);
                }
            }
        }
        public double HitDelay;
        public double Speed;
        public ProjectileType ProjectileType { get; set; }

        protected Animator animator;
        [SerializeField]
        protected double _timeExpired;
        [SerializeField]
        protected double _hitDelay;
        [SerializeField]
        protected int _hit;
        [SerializeField]
        protected Unit _target;
    }
}
