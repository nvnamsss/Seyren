using Seyren.System.Generics;
using Seyren.System.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.System.Damages
{
    /// <summary>
    /// The DamageEngine handles damage calculation and application according to the following flow:
    /// 1. Create Damage Event from a damage source
    /// 2. Apply pre-hit modifiers if any
    /// 3. Calculate base damage
    /// 4. Apply defense calculations if target has defense
    /// 5. Apply damage resistance if target has resistance
    /// 6. Calculate final damage value
    /// 7. Trigger 1st phase on-hit effects
    /// 8. Check if target is immune
    /// 9. Apply damage to target
    /// 10. Check if target is still alive
    /// 11. Trigger death event if target died, or update target state if alive
    /// 12. Trigger on-hit effects
    /// 13. End damage process
    /// </summary>
    public static class DamageEngine
    {
        // Events for damage system hooks
        public static event GameEventHandler<Damage> OnDamageCreated;
        public static event GameEventHandler<Damage> OnInflictedDamage;
        public static event EventHandler<DamageEventArgs> OnPreHitModifiersApplied;
        public static event EventHandler<DamageEventArgs> OnDamageCalculated;
        public static event EventHandler<DamageEventArgs> OnFirstPhaseEffects;
        public static event EventHandler<DamageEventArgs> OnDamageApplied;
        public static event EventHandler<DamageEventArgs> OnDeathTriggered;
        public static event EventHandler<DamageEventArgs> OnHitEffectsTriggered;

        /// <summary>
        /// Main entry point for damage calculation and application
        /// </summary>
        public static Damage DamageTarget(IUnit source, IUnit target, float baseDamage, DamageType type, TriggerType triggerType)
        {
            // Step 1: Create Damage Event
            Damage damage = new Damage
            {
                Source = source,
                Target = target,
                BaseDamage = baseDamage,
                DamageType = type,
                TriggerType = triggerType
            };

            OnDamageCreated?.Invoke(damage);
            
            // Exit if target is invulnerable
            if ((target.ObjectStatus | ObjectStatus.Invulnerable) != target.ObjectStatus)
            {
                Debug.Log($"Target {target} is invulnerable, no damage applied");
                return damage;
            }

            // Check and apply pre-hit modifiers
            ApplyPreHitModifiers(damage, source.GetModifiers());
            // Mitigate damage
            ApplyDamageResistance(damage, target.GetResistances());

            // Apply damage to target
            target.InflictDamage(damage);
            OnInflictedDamage?.Invoke(damage);

            if (damage.TriggerOnHitEffect) {
                TriggerOnHitEffects(target, source.GetOnHitEffects());
            }

            // End damage process
            return damage;
        }

        #region Implementation of each step
        
        private static void ApplyPreHitModifiers(Damage damageInfo, List<IModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                modifiers[i].Apply(damageInfo);
            }
        }


        private static void ApplyDamageResistance(Damage damageInfo, List<IResistance> resistances)
        {
            for (int i = 0; i < resistances.Count; i++)
            {
                resistances[i].Apply(damageInfo);
            }
        }
 
        
        private static void TriggerOnHitEffects(IUnit target, List<IOnHitEffect> onHitEffects)
        {
            // Apply on-hit effects
            foreach (var effect in onHitEffects)
            {
                effect.Trigger(target);
            }
        }
        
        #endregion
    }

    // Event args for damage events
    public class DamageEventArgs : EventArgs
    {
        public Damage DamageInfo { get; }

        public DamageEventArgs(Damage damageInfo)
        {
            DamageInfo = damageInfo;
        }
    }
}