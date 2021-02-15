using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Actions
{
    [Flags]
    public enum ActionType
    {
        None = 0,
        Walk,
        Run = 1,
        Jump,
        Dash,
        Roll,
        Attack = 2,
        Defense,
        CastAbility = 4,
        Channel,
        Interact,
        Dance,
        Sit,
        Grab,
    }
}
