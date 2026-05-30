using Seyren.System.Units;

namespace Seyren.System.Abilities
{
    public interface IAbilityInstance
    {
        /// <summary>The unit that cast this ability instance.</summary>
        IUnit Caster { get; }

        /// <summary>Seconds elapsed since this instance was created.</summary>
        float AliveTime { get; }

        /// <summary>Whether this instance is still producing effects.</summary>
        bool IsActive { get; }

        /// <summary>
        /// Advances the alive-time counter. Call once per tick from TickEffect.
        /// </summary>
        void Tick(float deltaTime);

        /// <summary>
        /// Stops this instance from producing further effects.
        /// Implementations should also clean up any in-flight objects (projectiles, VFX).
        /// </summary>
        void Cancel();
    }
}