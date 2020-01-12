using Base2D.System.UnitSystem.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base2D.System.AbilitySystem
{
    public abstract class ActiveAbility : Ability
    {
        public float BaseCastingTime { get; set; }
        public float TimeCastingLeft { get; set; }
        public bool IsCasting { get; set; }
        public BreakType BreakType { get; set; }
        protected abstract void DoCastAbility();

        public ActiveAbility(Unit caster, float castTime, float cooldown, int level) :
            base(caster, castTime, cooldown, level)
        {
            CastType = CastType.Active;
        }

        public override bool Cast()
        {
            if (!Condition())
            {
                return false;
            }

            Caster.StartCoroutine(Casting(TimeDelay, BaseCastingTime));
            return true;
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
                Caster.StartCoroutine(Casted(TimeDelay, BaseCoolDown));
            }
        }

        protected virtual IEnumerator Casted(float timeDelay, float timeCoolDown)
        {
            Active = false;
            IsCasting = false;
            TimeCoolDownLeft = timeCoolDown - timeDelay;

            while (TimeCoolDownLeft >= 0)
            {
                yield return new WaitForSeconds(timeDelay);
                TimeCoolDownLeft -= timeDelay;
            }
            Active = true;
        }
    }
}
