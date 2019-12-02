using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.UnitSystem.Projectile
{
    public interface IProjectile : IAttribute
    {
        double Speed { get; }
        double Angle { get; }
        double TimeExpired { get; set; }
        double HitDelay { get; set; }
        double ProjectileArc { get; set; }
        int MaxHit { get; set; }
        bool IsPenetrate { get; set; }
        ProjectileType Type { get; set; }
        IUnit Owner { get; set; }
        IUnit Target { get; set; }
        void Move();
        void Hit();
    }
}
