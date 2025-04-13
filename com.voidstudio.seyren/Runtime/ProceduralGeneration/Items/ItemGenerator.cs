using System;
using System.Collections.Generic;
using UnityEngine;
using Seyren.Algorithms;

namespace Seyren.ProceduralGeneration.Items
{
    /// <summary>
    /// Configuration for procedural item generation
    /// </summary>
    [Serializable]
    public class ItemGenerationConfig
    {
        public int MinRarity = 1;
        public int MaxRarity = 5;
        public int MinAttributes = 1;
        public int MaxAttributes = 4;
        public float AttributeChance = 0.7f;
    }    /// <summary>
    /// Base class for generating game items procedurally
    /// </summary>
    /// <typeparam name="T">The type of item to generate</typeparam>
    public abstract class ItemGenerator<T> : IGenerator<T>
    {
        protected ItemGenerationConfig config;
        protected Dictionary<string, RandomSet<string>> attributePools;

        public ItemGenerator(ItemGenerationConfig config)
        {
            this.config = config;
            this.attributePools = new Dictionary<string, RandomSet<string>>();
        }

        public virtual T Generate()
        {
            int rarity = UnityEngine.Random.Range(config.MinRarity, config.MaxRarity + 1);
            int attributeCount = UnityEngine.Random.Range(config.MinAttributes, config.MaxAttributes + 1);
            
            return GenerateWithParameters(rarity, attributeCount);
        }

        public virtual List<T> GenerateMultiple(int count)
        {
            var items = new List<T>();
            for (int i = 0; i < count; i++)
            {
                items.Add(Generate());
            }            return items;
        }
        
        public virtual void SetSeed(int seed)
        {
            UnityEngine.Random.InitState(seed);
        }

        /// <summary>
        /// Add a possible attribute to the generation pool
        /// </summary>
        public void AddAttributeToPool(string poolName, string attribute, float weight)
        {
            if (!attributePools.ContainsKey(poolName))
            {
                attributePools[poolName] = new RandomSet<string>();
            }
            attributePools[poolName].Add(attribute, weight);
        }

        /// <summary>
        /// Generate an item with specific parameters
        /// </summary>
        protected abstract T GenerateWithParameters(int rarity, int attributeCount);
    }
}
