using Seyren.System.Abilities;
using Seyren.System.Units.Dummies;
using Seyren.System.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.Example.Abilities
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
