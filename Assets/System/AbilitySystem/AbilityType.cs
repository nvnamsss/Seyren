using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.AbilitySystem
{
    [Flags]
    public enum AbilityType
    {
        CannotBreak = 1,
        Magical = 2,
        Pure = 4,
        OverTime = 8,
    }
}
