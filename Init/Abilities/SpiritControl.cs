using Base2D.System.AbilitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.Init.Abilities 
{
    public class SpiritControl : ChannelAbility
    {
        public override bool Cast()
        { 
            return false;
        }
        protected override bool Condition()
        {
            throw new NotImplementedException();
        }

        protected override void DoChannelAbility()
        {
            throw new NotImplementedException();
        }
    }
}
