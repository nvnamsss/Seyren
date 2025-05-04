using System.Collections.Generic;
using Seyren.System.Units;
using Seyren.Universe;
using UnityEngine;

namespace Seyren.System.Damages
{
    /// <summary>
    /// Manages all active damage over time effects in the game
    /// </summary>
    public class DamageOverTimeManager : ILoop
    {
        // Dictionary to track DOTs by target unit
        private Dictionary<IUnit, List<DamageOverTime>> _activeEffectsByUnit;
        
        // Dictionary to track DOTs by unique ID (for reference and stacking)
        private Dictionary<string, DamageOverTime> _activeEffectsById;

        public DamageOverTimeManager()
        {
            _activeEffectsByUnit = new Dictionary<IUnit, List<DamageOverTime>>();
            _activeEffectsById = new Dictionary<string, DamageOverTime>();
        }

        /// <summary>
        /// Apply a new DOT effect to a target
        /// </summary>
        public DamageOverTime ApplyEffect(string id, IUnit source, IUnit target, float damagePerTick, 
            DamageType damageType, float duration, float tickInterval, bool isStackable = false, int maxStacks = 1)
        {
            // Generate a unique instance ID if this is stackable
            string instanceId = id;
            if (isStackable)
            {
                instanceId = $"{id}_{source.ID}_{target.ID}";
            }

            // Check if this effect already exists
            if (_activeEffectsById.TryGetValue(instanceId, out DamageOverTime existingEffect))
            {
                if (existingEffect.IsStackable)
                {
                    existingEffect.AddStack();
                }
                existingEffect.Refresh();
                return existingEffect;
            }

            // Create new DOT effect
            var effect = new DamageOverTime(instanceId, source, target, damagePerTick, damageType, duration, tickInterval)
            {
                IsStackable = isStackable,
                MaxStacks = maxStacks,
                Name = id
            };

            // Add to tracking collections
            if (!_activeEffectsByUnit.ContainsKey(target))
            {
                _activeEffectsByUnit[target] = new List<DamageOverTime>();
            }
            
            _activeEffectsByUnit[target].Add(effect);
            _activeEffectsById[instanceId] = effect;

            return effect;
        }

        /// <summary>
        /// Update all active DOT effects
        /// </summary>
        public void Loop(ITime time)
        {
            // Create lists to track expired effects for removal
            var expiredEffects = new List<DamageOverTime>();
            var emptyUnits = new List<IUnit>();

            // Update each unit's DOTs
            foreach (var unitEntry in _activeEffectsByUnit)
            {
                var unit = unitEntry.Key;
                var effects = unitEntry.Value;

                // Skip if unit is no longer active
                if (unit == null)
                {
                    emptyUnits.Add(unit);
                    continue;
                }

                // Update each effect on this unit
                for (int i = 0; i < effects.Count; i++)
                {
                    var effect = effects[i];
                    bool isActive = effect.Update(time.DeltaTime);
                    
                    if (!isActive)
                    {
                        expiredEffects.Add(effect);
                    }
                }

                // Remove expired effects
                foreach (var effect in expiredEffects)
                {
                    effects.Remove(effect);
                    _activeEffectsById.Remove(effect.Id);
                }
                
                // Clear the list for reuse
                expiredEffects.Clear();
                
                // Mark unit for cleanup if no more effects
                if (effects.Count == 0)
                {
                    emptyUnits.Add(unit);
                }
            }

            // Clean up empty unit entries
            foreach (var unit in emptyUnits)
            {
                _activeEffectsByUnit.Remove(unit);
            }
        }

        /// <summary>
        /// Remove a specific effect by its ID
        /// </summary>
        public bool RemoveEffect(string effectId)
        {
            if (_activeEffectsById.TryGetValue(effectId, out DamageOverTime effect))
            {
                effect.Remove();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove all DOT effects from a target unit
        /// </summary>
        public void RemoveAllEffectsFromTarget(IUnit target)
        {
            if (_activeEffectsByUnit.TryGetValue(target, out List<DamageOverTime> effects))
            {
                foreach (var effect in effects)
                {
                    effect.Remove();
                    _activeEffectsById.Remove(effect.Id);
                }
                _activeEffectsByUnit.Remove(target);
            }
        }

        /// <summary>
        /// Get all active DOT effects on a unit
        /// </summary>
        public IReadOnlyList<DamageOverTime> GetEffectsOnUnit(IUnit target)
        {
            if (_activeEffectsByUnit.TryGetValue(target, out List<DamageOverTime> effects))
            {
                return effects.AsReadOnly();
            }
            return new List<DamageOverTime>().AsReadOnly();
        }

        /// <summary>
        /// Check if a unit has a specific DOT effect
        /// </summary>
        public bool HasEffect(IUnit target, string effectId)
        {
            if (_activeEffectsByUnit.TryGetValue(target, out List<DamageOverTime> effects))
            {
                return effects.Exists(e => e.Name == effectId);
            }
            return false;
        }
    }
}
