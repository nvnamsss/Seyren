using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.DamageSystem
{
    [Flags]
    public enum DamageType
    {
        Physical = 1,
        Magical = 2,
        Pure = 4,
        OverTime = 8,
    }
}
