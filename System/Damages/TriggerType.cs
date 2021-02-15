using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Damages
{
    [Flags]
    public enum TriggerType
    {
        None = 0,
        PrePassive = 1,
        Critical = 2,
        Evasion = 4,
        Reduction = 8,
        PostPassive = 16,
        All = ~0,
    }
}
