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
        public AudioClip Sound { get; set; }
        public GameObject BaseUnit { get; set; }
        public Animation BaseAnimation { get; set; }
        public float BaseCoolDown { get; set; }
        public float BaseCastingTime { get; set; }
        public float TimeCastingLeft { get; set; }
        //Default player cannt case any skill until he unlocked it!
        public bool IsCastable { get; set; }
        public bool IsCasting { get; set; }

        public GameObject ObjectTarget { get; set; }
        public GameObject PointTarget { get; set; }


        public void UnlockAbility()
        {
            IsCastable = true;
        }

        public void TrycastAbility(GameObject ObjectTarget, GameObject PointTarget)
        {
            this.ObjectTarget = ObjectTarget;
            this.PointTarget = PointTarget;
            if (IsCastable)
            {
                StartCoroutine(StartCasting(0, BaseCastingTime));
            }
            else
            {
                DoSomeThingIfCanntCasting(ObjectTarget, PointTarget);
            }
        }

        public override bool BreakAction(BreakType breakType)
        {
            if (IsCasting)
            {
                switch (breakType)
                {
                    case BreakType.CancleBreak:
                        StopAllCoroutines();
                        IsCasting = false;
                        IsCastable = true;
                        //Do something more
                        cancleBreakkAbility();
                        break;
                    case BreakType.KnockDownBreak:
                        StopAllCoroutines();
                        StartCoolDown();
                        //Do something more
                        knockDownBreakAbility();
                        break;
                    case BreakType.SpecialBreak:
                        //Special Action, Exp Soon release Ability
                        spectialBreakAbility();
                        return false;
                }
                IsCasting = false;
                return true;
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

        protected abstract void cancleBreakkAbility();

        protected abstract void knockDownBreakAbility();

        protected abstract void spectialBreakAbility();

        protected abstract void Initialize(GameObject obj);

        protected abstract void DoAnimation(GameObject PointTarget);

        protected abstract void DoCastAbility(GameObject ObjectTarget, GameObject PointTarget);

        protected abstract void DoSomeThingIfCanntCasting(GameObject ObjectTarget, GameObject PointTarget);
        
        IEnumerator StartCoolDown()
        {
            IsCastable = false;
            IsCasting = false;
            yield return new WaitForSeconds(BaseCoolDown);
            IsCastable = true;
        }

        IEnumerator StartCasting(float timeDelay, float timeLeft)
        {
            //Delay before Casting
            yield return new WaitForSeconds(timeDelay);
            IsCasting = true;
            TimeCastingLeft = timeLeft;

            while (BaseCastingTime >= 0)
            {
                yield return new WaitForSeconds(0.01f);
                BaseCastingTime -= 0.05f;
            }

            //Checking if cast still allow
            if (IsCasting)
            {
                IsCasting = false;
                DoCastAbility(ObjectTarget, PointTarget);
            }
        }
        
    }

}