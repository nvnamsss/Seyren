using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Seyren.System.Abilities
{
    public abstract class AuraAbility : Ability
    {
        public bool active;
        public float aoe;
        public float interval;
        public AuraAbility(float aoe, int level) : base(level)
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

        protected virtual async void OnActiveAsync(float interval) {
            int delay = (int)(interval * 1000);
            while (active) {
                AuraInterval();
                await Task.Delay(delay);
            }
        }


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
