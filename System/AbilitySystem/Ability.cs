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
        public AbilityType AbilityType { get; set; }
        public float BaseCoolDown { get; set; }
        public float TimeCoolDownLeft { get; set; }
        public float BaseCastingTime { get; set; }
        public float TimeCastingLeft { get; set; }

        //Default player cannt case any skill until he unlocked it!
        public bool IsCastable { get; set; }
        public bool IsCasting { get; set; }


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
        public abstract GameObject Create(Vector2 location, Quaternion rotation);
        protected int _level;

        public virtual bool UnlockAbility()
        {
            IsCastable = true;

            return IsCastable;
        }

        public Ability(Unit caster, float castTime, float cooldown, int level)
        {
            Caster = caster;
            BaseCastingTime = castTime;
            BaseCoolDown = cooldown;
            _level = level;
        }

        public abstract bool Cast();
        public abstract bool Cast(Unit caster);


        /// <summary>
        /// Main Cast Ability, call when Ability is release
        /// </summary>
        protected abstract void DoCastAbility();

        /// <summary>
        /// Làm gì đó nếu không thể Cast Skill
        /// </summary>

        protected virtual IEnumerator Casted(float timeDelay, float timeCoolDown)
        {
            IsCastable = false;
            IsCasting = false;
            TimeCoolDownLeft = timeCoolDown - timeDelay;

            while (TimeCoolDownLeft >= 0)
            {
                yield return new WaitForSeconds(timeDelay);
                TimeCoolDownLeft -= timeDelay;
            }
            IsCastable = true;
        }

        protected virtual IEnumerator Casting(float timeDelay, float timeCasting)
        {
            IsCasting = true;
            TimeCastingLeft = timeCasting;

            while (TimeCastingLeft >= 0)
            {
                yield return new WaitForSeconds(timeDelay);
                TimeCastingLeft -= timeDelay;
            }

            if (IsCasting)
            {
                IsCasting = false;
                DoCastAbility();
                Caster.StartCoroutine(Casted(0, BaseCoolDown));
            }
        }

    }

}