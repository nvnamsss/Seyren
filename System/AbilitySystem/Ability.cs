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
        /// Time between every process of an ability <br></br>
        /// TimeDelay is using on reduce cast time, cooldown, etc...
        /// </summary>
        public float TimeDelay { get; set; }
        public float TimeCoolDownLeft
        {
            get
            {
                return _timeCooldownLeft;
            }
            set
            {
                float original = _timeCooldownLeft;
                _timeCooldownLeft = value;

                if (original > 0 && _timeCooldownLeft <= 0)
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
                _active = value;

                if (_active != original)
                {
                    StatusChanged(this);
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
        protected float _timeCooldownLeft;
        protected int _level;

        public virtual bool UnlockAbility()
        {
            Active = true;

            return Active;
        }

        public Ability(Unit caster, float timeDelay, float cooldown, int level)
        {
            Caster = caster;
            TimeDelay = timeDelay;
            BaseCoolDown = cooldown;
            TimeCoolDownLeft = 0;
            _level = level;
        }

        public abstract bool Cast();
        /// <summary>
        /// Ability will be casted if condition is true
        /// </summary>
        /// <returns></returns>
        protected abstract bool Condition();
    }

}