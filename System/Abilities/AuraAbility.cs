using System.Collections;
using UnityEngine;

namespace Seyren.System.Abilities
{
    public abstract class AuraAbility : Ability
    {
        public bool active;
        public float aoe;
        public float interval;
        public AuraAbility(float aoe, int level) : base(0, level)
        {
            this.aoe = aoe;
            ManaCost = 0;
        }

        protected abstract void AuraInterval();
        // public override bool Cast()
        // {
        //     if (!Condition())
        //     {
        //         return false;
        //     }

        //     // Caster.StartCoroutine(OnActive(interval));

        //     return true;
        // }

        protected virtual IEnumerator OnActive(float interval)
        {
            while (active)
            {
                yield return new WaitForSeconds(interval);
                AuraInterval();
                // CooldownRemaining -= interval;
            }
        }

        protected void StatusChangedCallback(Ability sender)
        {
            // if (sender.Active)
            // {
            //     sender.Cast();
            // }
        }
    }
}
