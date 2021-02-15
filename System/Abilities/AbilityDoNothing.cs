using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Abilities
{
    public class AbilityDoNothing : Ability
    {
        public static readonly AbilityDoNothing Instance = new AbilityDoNothing();
        AbilityDoNothing() : base(null, 1, 1)
        {
            Active = false;
        }

        public override bool Cast()
        {
            return false;
        }

        protected override bool Condition()
        {
            return false;
        }

        protected override bool UnlockCondition()
        {
            return false;
        }
    }
}
