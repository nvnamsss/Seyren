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
    public abstract class AuraAbility : Ability
    {
        public float AoE;
        public float Interval;
        public AuraAbility(Unit caster, float aoe, int level) : base(caster, 0, level)
        {
            AoE = aoe;
            ManaCost = 0;
            StatusChanged += StatusChangedCallback;
        }

        protected abstract override bool Condition();
        protected abstract void AuraInterval();
        public override bool Cast()
        {
            if (!Condition())
            {
                return false;
            }

            Caster.StartCoroutine(OnActive(Interval));

            return true;
        }

        protected virtual IEnumerator OnActive(float interval)
        {
            while (base.Active)
            {
                yield return new WaitForSeconds(interval);
                AuraInterval();
                CooldownRemaining -= interval;
            }
        }

        protected void StatusChangedCallback(Ability sender)
        {
            if (sender.Active)
            {
                sender.Cast();
            }
        }
    }
}
