using Crom.System.UnitSystem;
using Crom.System.ActionSystem;
using Crom.System.ActionSystem.BreakAtion;
using Crom.System.ActionSystem.DelayAction;
using Crom.System.BuffSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crom.System.AbilitySystem
{

    public abstract class Ability : IAction, IAbility
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
        public GameObject PointTarget { get; set; }
        
        public void UnlockAbility()
        {
            IsCastable = true;
        }

        public bool TryCastAbility(GameObject ObjectTarget, GameObject PointTarget)
        {
            this.ObjectTarget = ObjectTarget;
            this.PointTarget = PointTarget;
            if (IsCastable)
            {
                StartCoroutine(StartCasting(0, BaseCastingTime));
                return true;
            }
            else
            {
                DoSomeThingIfCannotCasting(ObjectTarget, PointTarget);
                return false;
            }
        }

        public override bool BreakAction(BreakType breakType)
        {
            if (IsCasting)
            {
                switch (breakType)
                {
                    case BreakType.CancleBreak:
                        return CancleBreakkAbility();
                    case BreakType.KnockDownBreak:
                        return KnockDownBreakAbility();
                    case BreakType.SpecialBreak:
                        return SpectialBreakAbility();
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

        protected bool CancleBreakkAbility()
        {
            switch (AbilityType)
            {
                case AbilityType.CannotCancle:
                    return false;
                case AbilityType.CanCancleNoCoolDown:
                    StopAllCoroutines();
                    StartCoroutine(StartCoolDown(0, 0));
                    return true;
                case AbilityType.CanCancleWithCoolDown:
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
                    DoCastAbility(ObjectTarget, PointTarget);
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

        protected abstract bool SpectialBreakAbility();

        /// <summary>
        /// Init Ability Unit
        /// </summary>
        /// <param name="ObjectUnit">Unit Init</param>
        /// <param name="ObjectTarget">Source Tranform</param>
        protected abstract void Initialize(GameObject ObjectUnit, GameObject ObjectTarget);

        /// <summary>
        /// Change Unit Animation
        /// </summary>
        /// <param name="PointTarget">Target</param>
        protected abstract void DoAnimation(GameObject PointTarget);
        
        /// <summary>
        /// Main Cast Ability, call when Ability is release
        /// </summary>
        /// <param name="ObjectTarget">Source Object</param>
        /// <param name="PointTarget">Target Object</param>
        protected abstract void DoCastAbility(GameObject ObjectTarget, GameObject PointTarget);

        /// <summary>
        /// Làm gì đó nếu không thể Cast Skill
        /// </summary>
        /// <param name="ObjectTarget"></param>
        /// <param name="PointTarget"></param>
        protected abstract void DoSomeThingIfCannotCasting(GameObject ObjectTarget, GameObject PointTarget);
        
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
                DoCastAbility(ObjectTarget, PointTarget);
                StartCoroutine(StartCoolDown(0, BaseCoolDown));
            }
        }
        
    }

}