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
        CannotCancle = 2,
        CannotGetKnockDown = 4,
        CanKnockDownWithSoonRelease = 8,
        CanKnockDown = 16,
        CanCancleNoCoolDown = 32,
        CanCancleWithCoolDown = 64,
        All = ~0,
    }
}
