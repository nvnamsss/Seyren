using System;
using Seyren.System.Units;
using Seyren.Universe;

namespace Seyren.Projectiles
{
    public interface IProjectile : IObject, ILoop
    {
        public string Type { get; }
        public float Speed { get; set; }
        public float LifeTime { get; set; }
        public event Action<IProjectile> OnCompleted;
        
        // Kill the projectile
        public void Revoke();
    }
}