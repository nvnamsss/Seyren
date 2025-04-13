using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.ProceduralGeneration.Items
{
    /// <summary>
    /// Configuration specific to weapon generation
    /// </summary>
    [Serializable]
    public class WeaponGenerationConfig : ItemGenerationConfig
    {
        public float MinDamage = 1;
        public float MaxDamage = 100;
        public float DamageScalingFactor = 1.5f;
    }

    /// <summary>
    /// Example implementation of a weapon generator
    /// </summary>
    public class WeaponGenerator : ItemGenerator<Weapon>
    {
        protected WeaponGenerationConfig weaponConfig;

        public WeaponGenerator(WeaponGenerationConfig config) : base(config)
        {
            this.weaponConfig = config;

            // Initialize attribute pools with default weapon attributes
            AddAttributeToPool("Prefix", "Sharp", 1.0f);
            AddAttributeToPool("Prefix", "Rusty", 0.7f);
            AddAttributeToPool("Prefix", "Ancient", 0.5f);
            AddAttributeToPool("Prefix", "Legendary", 0.1f);

            AddAttributeToPool("Element", "Fire", 0.4f);
            AddAttributeToPool("Element", "Ice", 0.4f);
            AddAttributeToPool("Element", "Lightning", 0.3f);
            AddAttributeToPool("Element", "Holy", 0.1f);

            AddAttributeToPool("Suffix", "of Power", 0.5f);
            AddAttributeToPool("Suffix", "of the Whale", 0.3f);
            AddAttributeToPool("Suffix", "of Swiftness", 0.4f);
            AddAttributeToPool("Suffix", "of the Gods", 0.1f);
        }

        protected override Weapon GenerateWithParameters(int rarity, int attributeCount)
        {
            // Create base weapon
            var weapon = new Weapon();
            
            // Generate base damage based on rarity
            float rarityMultiplier = Mathf.Pow(weaponConfig.DamageScalingFactor, rarity - 1);
            float baseDamage = UnityEngine.Random.Range(weaponConfig.MinDamage, weaponConfig.MaxDamage);
            weapon.Damage = baseDamage * rarityMultiplier;

            // Add random attributes based on attributeCount
            List<string> attributes = new List<string>();

            if (attributeCount > 0 && attributePools["Prefix"].Get() is string prefix)
            {
                attributes.Add(prefix);
            }

            if (attributeCount > 1 && UnityEngine.Random.value < config.AttributeChance 
                && attributePools["Element"].Get() is string element)
            {
                attributes.Add(element);
                weapon.ElementalType = element;
            }

            if (attributeCount > 2 && attributePools["Suffix"].Get() is string suffix)
            {
                attributes.Add(suffix);
            }

            // Construct weapon name from attributes
            weapon.Name = string.Join(" ", attributes);

            // Set rarity
            weapon.Rarity = rarity;

            return weapon;
        }
    }

    /// <summary>
    /// Basic weapon class - extend this based on your game's needs
    /// </summary>
    public class Weapon
    {
        public string Name { get; set; }
        public float Damage { get; set; }
        public int Rarity { get; set; }
        public string ElementalType { get; set; }
    }
}
