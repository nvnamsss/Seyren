using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Dummies;
using Base2D.System.UnitSystem.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.Example.Abilities
{
    public class IceShield : ToggleAbility
    {
        private Dummy shield;
        public IceShield(Unit caster) : base(caster)
        {
        }

        protected override bool Condition()
        {
            return true;
        }

        protected override void DoCastAbility()
        {

        }

        protected override bool UnlockCondition()
        {
            return true;
        }
    }
}
