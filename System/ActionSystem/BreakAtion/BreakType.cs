using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.ActionSystem.BreakAtion
{
    [Flags]
    public enum BreakType
    {
        CancelBreak = 1,
        KnockDownBreak = 2,
        SpecialBreak = 4,
        All = ~0,
    }
}
