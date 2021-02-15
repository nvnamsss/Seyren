using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Units.Projectiles
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
        /// <summary>
        /// The projectile with trajectory is not controlled by developer instead of physic <br></br>
        /// </summary>
        Custom,
    }
}
