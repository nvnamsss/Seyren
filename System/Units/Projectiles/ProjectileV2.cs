using System.Numerics;

namespace Seyren.System.Units.Projectiles {
    public interface IProjectile : IObject {
        // public delegate void OnHitHandler(Projectile sender);
        // public delegate void OnCreateHandler(Projectile sender);
        // public delegate void OnDestroyHandler(Projectile sender);

        // public event OnHitHandler OnHit;
        // public event OnCreateHandler OnCreate;
        // public event OnDestroyHandler OnDestroy;

        public abstract void Create(Vector3 position, Vector3 direction);
        public abstract void Destroy();
    }

}