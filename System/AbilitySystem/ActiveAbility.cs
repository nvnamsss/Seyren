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
        public bool IsCasting { get; set; }
        public BreakType BreakType { get; set; }

        public ActiveAbility(Unit caster, float castTime, float cooldown, int level) :
            base(caster, castTime, cooldown, level)
        {

        }

        public override bool Cast()
        {
            if (!Condition())
            {
                return false;
            }

            return true;
        }

        protected abstract void DoCastAbility();

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
    }
}
