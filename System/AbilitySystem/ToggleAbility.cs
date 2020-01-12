using Base2D.System.UnitSystem.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.AbilitySystem
{
    public abstract class ToggleAbility : Ability
    {
        public bool IsOn;
        public ToggleAbility(Unit caster) : base(caster, 0, 0, 1)
        {
            IsOn = true;
        }

        public virtual bool Cast()
        {
            if (!Condition())
            {
                return false;
            }

            return true;
        }
    }
}