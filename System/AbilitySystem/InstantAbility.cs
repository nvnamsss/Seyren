using Base2D.System.Generic;
using Base2D.System.UnitSystem.Units;
using System.Collections;
using UnityEngine;

namespace Base2D.System.AbilitySystem
{
    public abstract class InstantAbility : Ability
    {
        public event GameEventHandler<InstantAbility> Casted;
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

            Casted?.Invoke(this);
            DoCastAbility();
            cooldownCoroutine = Caster.StartCoroutine(CastedProcess(CooldownInterval, BaseCoolDown));
            return true;
        }

        protected virtual IEnumerator CastedProcess(float timeDelay, float cooldown)
        {
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
