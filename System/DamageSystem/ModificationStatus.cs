using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.DamageSystem
{
    [Flags()]
    public enum ModificationStatus
    {
        Common = 0,
        Critical = 1,
        Evade = 2,
        Blocked = 4,
        Reduced = 8,
        Parry = 16,
    }
}
