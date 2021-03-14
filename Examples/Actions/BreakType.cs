using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.Examples.Actions
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
