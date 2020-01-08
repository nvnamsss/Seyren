using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Units;
using System.Collections;
using UnityEngine;

namespace Base2D.Init.Abilities
{
    public class AncientSlam : Ability
    {
        public static readonly int Id = 0x65678301;
        private Unit unit;
        public AncientSlam(Unit u)
        {
            unit = u;
            BaseCoolDown = 10;
            BaseCastingTime = 2;
        }

        public override bool Cast()
        {
            if (TimeCoolDownLeft > 0 || IsCasting)
            {
                return false;
            }

            unit.Action.Animator.SetBool("Spell", true);
            TimeCastingLeft = BaseCastingTime;
            unit.StartCoroutine(Casting(0.5f, BaseCastingTime));
            return true;
        }

        IEnumerator Casting(float timeDelay, float timeCasting)
        {
            yield return new WaitForSeconds(timeDelay);
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
            }

        }


        public override GameObject Create(Vector2 location, Quaternion rotation)
        {
            return null;
        }

        protected override void DoCastAbility()
        {
            IsCasting = false;
            unit.Action.Animator.SetBool("Spell", false);

            TimeCoolDownLeft = BaseCoolDown;
            unit.StartCoroutine(StartCoolDown(Time.deltaTime, BaseCoolDown));
        }
    }
}
