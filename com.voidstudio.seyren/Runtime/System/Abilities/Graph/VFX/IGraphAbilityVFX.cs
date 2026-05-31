using UnityEngine;

namespace Seyren.System.Abilities
{
    /// <summary>
    /// Provides visual effects for a GraphAbility without modifying the ability itself.
    ///
    /// Implementations are registered on a GraphAbility via AddVFX() and form a
    /// chain-of-responsibility: ability-level VFX are consulted first, followed by
    /// modifier-registered fallbacks. A handler that does not own a given entity key
    /// returns null (CreateVisual) or false (event hooks) to pass control to the next
    /// handler in the chain.
    ///
    /// Entity keys are the stable contract between an ability and its VFX:
    ///   - CreateVisual   — supply the GameObject for a spawned entity
    ///   - OnEntitySpawned  — attach particles, trails, or other effects after spawn
    ///   - OnEntityCompleted — play hit/burst/expire effects when an entity is revoked
    /// </summary>
    public interface IGraphAbilityVFX
    {
        /// <summary>
        /// Returns a GameObject to use as the visual for the given entity key,
        /// or null if this handler does not own the key (try the next handler).
        /// </summary>
        GameObject CreateVisual(string entityKey, AbilityGraphContext ctx);

        /// <summary>
        /// Called immediately after an entity is spawned and stored in the context.
        /// Return true if handled (stops the chain), false to pass to the next handler.
        /// </summary>
        bool OnEntitySpawned(string entityKey, object entity, AbilityGraphContext ctx);

        /// <summary>
        /// Called when an entity's lifetime ends (hit or expiry via IProjectile.OnCompleted).
        /// Return true if handled (stops the chain), false to pass to the next handler.
        /// </summary>
        bool OnEntityCompleted(string entityKey, object entity, AbilityGraphContext ctx);
    }
}
