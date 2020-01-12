using Base2D.System.UnitSystem.Units;
using System.Collections;

namespace Base2D.System.AbilitySystem
{
    public abstract class InstantAbility : Ability
    {
        public InstantAbility(Unit caster, float cooldown, int level) : base(caster, 0, cooldown, 1)
        {
            CastType = CastType.Instant;
        }

        public virtual bool Cast()
        {
            if (!Condition())
            {
                return false;
            }

            return true;
        }

        protected abstract override bool Condition();

        protected abstract override void DoCastAbility();

        protected override IEnumerator Casting(float timeDelay, float timeCasting)
        {
            DoCastAbility();
            Caster.StartCoroutine(Casted(0, BaseCoolDown));
            yield break;
        }
    }
}
