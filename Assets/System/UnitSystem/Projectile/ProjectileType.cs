using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.UnitSystem.Projectile
{
    public enum ProjectileType
    {
        None,
        /// <summary>
        /// The projectile is light and focus on short range or curver
        /// </summary>
        Arrow,
        /// <summary>
        /// The projectile is none affected by gravity focus on straight
        /// </summary>
        Missile,
        /// <summary>
        /// The projectile is very fast, instantly hit
        /// </summary>
        Laser,
        /// <summary>
        /// The projectile
        /// </summary>
        Homing,
        Custom,
    }
}
