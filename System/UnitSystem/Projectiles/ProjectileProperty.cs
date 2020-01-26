﻿using System.Collections;
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
        public BoxCollider2D Collider { get; set; }
        public Rigidbody2D Body { get; set; }
        public int MaxHit { get; set; }
        public bool IsPenetrate { get; set; }
        public Unit Owner { get; set; }
        public Attribute Attribute { get; set; }
        public ModificationInfos Modification { get; set; }

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
                    Active = false;
                    //if (animator != null && animator.isInitialized)
                    //{
                    //    animator.SetBool("Death", true);
                    //}
                    TimeExpired?.Invoke(this);
                }
            }
        }
        public bool Active;
        public double HitDelay;
        public double Speed;
        public ProjectileType ProjectileType => _projectileType;

        protected Animator animator;
        [SerializeField]
        protected double _timeExpired;
        [SerializeField]
        protected double _hitDelay;
        [SerializeField]
        protected int _hit;
        protected ProjectileType _projectileType;
    }
}
