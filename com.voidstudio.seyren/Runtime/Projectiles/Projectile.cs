using Seyren.System.Units;
using Seyren.Universe;

namespace Seyren.Projectiles
{
    public interface IProjectile : IObject, ILoop
    {

        public string Type { get; set; }
        public float Speed { get; set; }
        public float LifeTime { get; set; }
        public bool IsActive { get;}
        // Kill the projectile
        public void Revoke();
    }
}