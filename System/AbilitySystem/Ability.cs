using Base2D.System.UnitSystem;
using Base2D.System.ActionSystem;
using Base2D.System.ActionSystem.BreakAtion;
using Base2D.System.ActionSystem.DelayAction;
using Base2D.System.BuffSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base2D.System.AbilitySystem
{

    public abstract class Ability : Action, IAbility
    {
        public AbilityType AbilityType { get; set; }

        public AudioClip Sound { get; set; }
        public GameObject BaseUnit { get; set; }
        public Animation BaseAnimation { get; set; }

        public float BaseCoolDown { get; set; }
        public float TimeCoolDownLeft { get; set; }
        public float BaseCastingTime { get; set; }
        public float TimeCastingLeft { get; set; }

        public float TimeUpdate { get; set; }

        //Default player cannt case any skill until he unlocked it!
        public bool IsCastable { get; set; }
        public bool IsCasting { get; set; }

        public GameObject ObjectTarget { get; set; }
        public Vector3 PointTarget { get; set; }
        
        public void UnlockAbility()
        {
            IsCastable = true;
        }

        public bool TryCastAbility(GameObject ObjectTarget)
        {
            this.ObjectTarget = ObjectTarget;
            if (IsCastable)
            {
                StartCoroutine(StartCasting(0, BaseCastingTime));
                return true;
            }
            else
            {
                DoSomeThingIfCannotCasting();
                return false;
            }
        }

        public bool TryCastAbility(Vector3 point)
        {
            PointTarget = point;
            if (IsCastable)
            {
                StartCoroutine(StartCasting(0, BaseCastingTime));
                return true;
            }
            else
            {
                DoSomeThingIfCannotCasting();
                return false;
            }
        }
        public override bool BreakAction(BreakType breakType)
        {
            if (IsCasting)
            {
                switch (breakType)
                {
                    case BreakType.CancelBreak:
                        return CancelBreakAbility();
                    case BreakType.KnockDownBreak:
                        return KnockDownBreakAbility();
                    case BreakType.SpecialBreak:
                        return SpecialBreakAbility();
                }
            }
            return false;
        }

        //Default DelayAction
        public override bool DelayAction(DelayInfo delayInfo)
        {
            if (IsCasting)
            {
                StopAllCoroutines();
                StartCoroutine(StartCasting(delayInfo.DelayTime, TimeCastingLeft));
            }
            return true;
        }

        protected bool CancelBreakAbility()
        {
            switch (AbilityType)
            {
                case AbilityType.CannotCancel:
                    return false;
                case AbilityType.CanCancelNoCoolDown:
                    StopAllCoroutines();
                    StartCoroutine(StartCoolDown(0, 0));
                    return true;
                case AbilityType.CanCancelWithCoolDown:
                    StopAllCoroutines();
                    StartCoroutine(StartCoolDown(0, BaseCoolDown));
                    return true;
            }

            return false;
        }

        protected bool KnockDownBreakAbility()
        {

            switch (AbilityType)
            {
                case AbilityType.CanKnockDownWithSoonRelease:
                    DoCastAbility();
                    StartCoroutine(StartCoolDown(0, BaseCoolDown));
                    return true;
                case AbilityType.CanKnockDown:
                    StopAllCoroutines();
                    StartCoroutine(StartCoolDown(0, BaseCoolDown));
                    return true;
                case AbilityType.CannotGetKnockDown:
                    return false;
            }
            return false;
        }

        protected abstract bool SpecialBreakAbility();

        /// <summary>
        /// Init Ability Unit
        /// </summary>

        protected abstract void Initialize();

        /// <summary>
        /// Change Unit Animation
        protected abstract void DoAnimation();
        
        /// <summary>
        /// Main Cast Ability, call when Ability is release
        /// </summary>
        protected abstract void DoCastAbility();

        /// <summary>
        /// Làm gì đó nếu không thể Cast Skill
        /// </summary>
        protected abstract void DoSomeThingIfCannotCasting();
        
        IEnumerator StartCoolDown(float timeDelay, float timeCoolDown)
        {
            yield return new WaitForSeconds(timeDelay);
            IsCastable = false;
            IsCasting = false;
            TimeCoolDownLeft = timeCoolDown;

            while (TimeCoolDownLeft >= 0)
            {
                yield return new WaitForSeconds(TimeUpdate);
                TimeCoolDownLeft -= TimeUpdate;
            }
            IsCastable = true;
        }

        IEnumerator StartCasting(float timeDelay, float timeCasting)
        {
            //Delay before Casting
            yield return new WaitForSeconds(timeDelay);
            IsCasting = true;
            TimeCastingLeft = timeCasting;

            while (TimeCastingLeft >= 0)
            {
                yield return new WaitForSeconds(TimeUpdate);
                TimeCastingLeft -= TimeUpdate;
            }

            //Check if cast still allow
            if (IsCasting)
            {
                IsCasting = false;
                DoCastAbility();
                StartCoroutine(StartCoolDown(0, BaseCoolDown));
            }
        }
        
    }

}
