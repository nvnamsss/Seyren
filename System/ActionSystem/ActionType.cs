using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.ActionSystem
{
    [Flags]
    public enum ActionType
    {
        None = 0,
        Moving = 1,
        Attack = 2,
        CastAbility = 4,
    }
}
