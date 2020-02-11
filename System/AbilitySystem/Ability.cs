using System.Collections.Generic;
using System.Collections;
using Base2D.System.ActionSystem.BreakAtion;
using Base2D.System.ActionSystem.DelayAction;
using Base2D.System.ActionSystem;
using Base2D.System.BuffSystem;
using Base2D.System.UnitSystem.Units;
using Base2D.System.UnitSystem;
using UnityEngine;
using System;

namespace Base2D.System.AbilitySystem
{
    [Serializable]
    public abstract class Ability
    {
        public static readonly Ability DoNothing = AbilityDoNothing.Instance;
        public static float MaxInterval = 4;
        public static float MinInterval = 1;
        public delegate void StatusChangedHandler(Ability sender);
        public delegate void CooldownCompletedHandler(Ability sender);
        public event StatusChangedHandler StatusChanged;
        /// <summary>
        /// Trigger when ability cooldown is done and ability is ready to use
        /// </summary>
        public event CooldownCompletedHandler CooldownCompleted;
        public CastType CastType { get; protected set; }
        public float BaseCoolDown { get; set; }
        /// <summary>
        /// Time between every process for cooldown of an ability <br></br>
        /// </summary>
        public float CooldownInterval { get; set; }
        public float CooldownRemaining
        {
            get
            {
                return _cooldownRemaining;
            }
            set
            {
                float original = _cooldownRemaining;
                _cooldownRemaining = value;

                if (original > 0 && _cooldownRemaining <= 0)
                {
                    CooldownCompleted?.Invoke(this);
                }
            }
        }
        public float ManaCost { get; set; }
        /// <summary>
        /// Status of ability
        /// </summary>
        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                bool original = _active;
                bool unlock = UnlockCondition();
                if (!unlock)
                {
                    return;
                }

                _active = value;

                if (_active != original)
                {
                    StatusChanged?.Invoke(this);
                }
            }
        }
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }
        [SerializeField]
        public Unit Caster;
        public Unit Target;
        public Vector3 PointTarget;
        protected bool _active;
        protected float _cooldownRemaining;
        protected int _level;
        public virtual bool UnlockAbility()
        {
            Active = true;
            
            return Active;
        }

        public Ability(Unit caster, float cooldown, int level)
        {
            Caster = caster;
            if (cooldown > MaxInterval)
                CooldownInterval = MaxInterval / 10;
            else if (cooldown < MinInterval)
                CooldownInterval = cooldown;
            else
                CooldownInterval = cooldown / 10;
            BaseCoolDown = cooldown;
            CooldownRemaining = 0;
            _level = level;
        }

        public abstract bool Cast();
        /// <summary>
        /// Ability will be casted if condition is true
        /// </summary>
        /// <returns></returns>
        protected abstract bool Condition();
        protected abstract bool UnlockCondition();
    }

}