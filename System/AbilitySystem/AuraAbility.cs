using Base2D.System.UnitSystem.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.AbilitySystem
{
    public abstract class AuraAbility : Ability
    {
        public float AOE;
        public AuraAbility(Unit caster, int level) : base(caster, 0, 0, 1)
        {
            ManaCost = 0;
        }

        protected abstract override bool Condition();

        protected abstract override void DoCastAbility();
    }
}
