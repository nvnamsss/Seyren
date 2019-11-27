using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.System.UnitSystem
{
    [Flags]
    public enum UnitStatus
    {
        Slow,
        Knockback,
        Stun,
        Invulnerable,
        SpellImmunity,
    }
}
