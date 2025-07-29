using System;

namespace Seyren.System.Actions
{
    [Flags]
    public enum ActionType
    {
        None = 0,
        Moving = 1 >> 1,
        Dashing = 1 >> 2,
        Rolling = 1 >> 3,
        Attacking = 1 >> 4,
        Defending = 1 >> 5,
        Casting = 1 >> 6,
        Channeling = 1 >> 7,
        Interacting = 1 >> 8,
    }
}
