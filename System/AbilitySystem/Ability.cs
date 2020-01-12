using System.Collections.Generic;
using System.Collections;
using Base2D.System.ActionSystem.BreakAtion;
using Base2D.System.ActionSystem.DelayAction;
using Base2D.System.ActionSystem;
using Base2D.System.BuffSystem;
using Base2D.System.UnitSystem.Units;
using Base2D.System.UnitSystem;
using UnityEngine;

namespace Base2D.System.AbilitySystem
{
    public abstract class Ability
    {
        public delegate void CooldownCompletedHandler(Ability sender);
        public event CooldownCompletedHandler CooldownCompleted;
        public CastType CastType { get; protected set; }
        public float BaseCoolDown { get; set; }
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
        //Default player cannt case any skill until he unlocked it!
        public bool IsCastable { get; set; }


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

        public Unit Caster;
        public Unit Target;
        public Vector3 PointTarget;
        protected float _timeCooldownLeft;
        protected int _level;

        public virtual bool UnlockAbility()
        {
            IsCastable = true;

            return IsCastable;
        }

        public Ability(Unit caster, float castTime, float cooldown, int level)
        {
            Caster = caster;
            BaseCoolDown = cooldown;
            _level = level;
        }


        public abstract bool Cast();

        protected abstract bool Condition();
        /// <summary>
        /// Main Cast Ability, call when Ability is release
        /// </summary>
    }

}