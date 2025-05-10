using UnityEngine;

namespace Seyren.System.Formulas
{
    /// <summary>
    /// Provides damage calculation formulas based on Diablo games
    /// </summary>
    public static class DiabloFormula
    {
        /// <summary>
        /// Calculates damage received after applying armor damage reduction (Diablo 2 style)
        /// </summary>
        /// <param name="incomingDamage">Base damage before reduction</param>
        /// <param name="armor">Target's armor value</param>
        /// <param name="attackerLevel">Level of the attacker</param>
        /// <returns>Final damage after armor reduction</returns>
        public static float CalculateDamageWithArmor(float incomingDamage, float armor, int attackerLevel)
        {
            // Diablo 2 armor formula: damage reduction = armor / (armor + 50 * attackerLevel)
            // Reduction is capped at 80%
            float reduction = armor / (armor + 50f * attackerLevel);
            reduction = Mathf.Clamp(reduction, 0f, 0.8f); // Cap at 80% damage reduction
            
            return incomingDamage * (1f - reduction);
        }

        /// <summary>
        /// Calculates damage received after applying elemental resistance
        /// </summary>
        /// <param name="incomingDamage">Base elemental damage</param>
        /// <param name="resistance">Target's resistance value (0-100)</param>
        /// <returns>Final damage after resistance reduction</returns>
        public static float CalculateDamageWithResistance(float incomingDamage, float resistance)
        {
            // Clamp resistance between -100 (double damage) and 75 (75% reduction)
            float effectiveResistance = Mathf.Clamp(resistance, -100f, 75f);
            
            return incomingDamage * (1f - effectiveResistance / 100f);
        }

        /// <summary>
        /// Calculates the final damage received after applying both armor and resistances
        /// </summary>
        /// <param name="incomingDamage">Base damage before reduction</param>
        /// <param name="armor">Target's armor value</param>
        /// <param name="resistance">Target's resistance to this damage type (0-100)</param>
        /// <param name="attackerLevel">Level of the attacker</param>
        /// <param name="damageTypeIsPhysical">Whether the damage is physical (true) or elemental (false)</param>
        /// <returns>Final damage after all reductions</returns>
        public static float CalculateDamageReceived(float incomingDamage, float armor, float resistance, int attackerLevel, bool damageTypeIsPhysical = true)
        {
            float damage = incomingDamage;
            
            // Apply armor reduction only for physical damage
            if (damageTypeIsPhysical)
            {
                damage = CalculateDamageWithArmor(damage, armor, attackerLevel);
            }
            
            // Apply resistance (always, for both physical and elemental)
            damage = CalculateDamageWithResistance(damage, resistance);
            
            return Mathf.Max(0, damage); // Ensure damage is never negative
        }

        /// <summary>
        /// Calculates critical hit damage
        /// </summary>
        /// <param name="baseDamage">Base damage before critical calculation</param>
        /// <param name="critMultiplier">Critical hit damage multiplier (default 1.5 = 150% damage)</param>
        /// <returns>Critical hit damage</returns>
        public static float CalculateCriticalDamage(float baseDamage, float critMultiplier = 1.5f)
        {
            return baseDamage * critMultiplier;
        }

        /// <summary>
        /// Checks if an attack is a critical hit based on critical chance
        /// </summary>
        /// <param name="criticalChance">Chance to land a critical hit (0.0-1.0)</param>
        /// <returns>True if the attack is a critical hit</returns>
        public static bool IsCriticalHit(float criticalChance)
        {
            return Random.value <= Mathf.Clamp01(criticalChance);
        }

        /// <summary>
        /// Diablo-style diminishing returns calculation for defensive stats
        /// </summary>
        /// <param name="value">Raw defensive value</param>
        /// <param name="diminishingFactor">Factor controlling how quickly returns diminish</param>
        /// <returns>Effective value after diminishing returns</returns>
        public static float ApplyDiminishingReturns(float value, float diminishingFactor = 0.01f)
        {
            return value / (1 + diminishingFactor * value);
        }
    }
}