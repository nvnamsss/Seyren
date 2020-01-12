using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.AbilitySystem
{
    public enum CastType
    {
        /// <summary>
        /// Ability that cast for a while and the effect is occur while channeling
        /// </summary>
        Channel,
        /// <summary>
        /// Ability with casting time, the effect will be occured after cast success
        /// </summary>
        Active,
        /// <summary>
        /// Ability with out casting time (instant cast)
        /// </summary>
        Instant,
        /// <summary>
        /// Ability affect an area with out cast or mana cost
        /// </summary>
        Aura,
        Autocast,
        /// <summary>
        /// Ability with 2 state On or Off
        /// </summary>
        Toggle,
    }
}
