using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Damages
{
        [Flags]
    public enum DamageType
    {
        Physical = 1,
        Magical = 2,
        Pure = 4,
    }

    public enum StackType
    {
        None,
        Unique,
        Diminishing,
        ByHalf,
        Directly,
    }

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
