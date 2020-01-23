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
        public delegate void CastingSpellHandler(ActiveAbility sender, CastingSpellEventArgs e);
        public delegate void CastedSpellHandler(ActiveAbility sender);
        public event CastingSpellHandler Casting;
        public event CastedSpellHandler Casted;
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

            CastingSpellEventArgs cing = new CastingSpellEventArgs();
            Casting?.Invoke(this, cing);

            if (cing.Cancel)
            {
                return false;
            }

            Caster.StartCoroutine(CastingProcess(TimeDelay, BaseCastingTime));
            return true;
        }

        protected virtual IEnumerator CastingProcess(float timeDelay, float timeCasting)
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
                Caster.StartCoroutine(CastedProcess(TimeDelay, BaseCoolDown));
            }
        }

        protected virtual IEnumerator CastedProcess(float timeDelay, float timeCoolDown)
        {
            Casted?.Invoke(this);
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
