using Base2D.System.UnitSystem.Units;
using System.Collections;
using UnityEngine;

namespace Base2D.System.AbilitySystem
{
    public abstract class InstantAbility : Ability
    {
        protected Coroutine cooldownCoroutine;
        protected abstract override bool Condition();

        protected abstract void DoCastAbility();
        public InstantAbility(Unit caster, float cooldown, int level) : base(caster, cooldown, 1)
        {
            CastType = CastType.Instant;
        }

        public override bool Cast()
        {
            if (!Condition())
            {
                return false;
            }

            cooldownCoroutine = Caster.StartCoroutine(Casted(CooldownInterval, BaseCoolDown));
            return true;
        }

        protected virtual IEnumerator Casted(float timeDelay, float cooldown)
        {
            DoCastAbility();
            CooldownRemaining = cooldown - timeDelay;

            while (CooldownRemaining >= 0)
            {
                yield return new WaitForSeconds(timeDelay);
                CooldownRemaining -= timeDelay;
            }
            yield break;
        }
    }
}
