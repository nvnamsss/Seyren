using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.ProceduralGeneration
{
    /// <summary>
    /// Base interface for all procedural generators in the framework
    /// </summary>
    public interface IGenerator<T>
    {
        /// <summary>
        /// Generate a single instance
        /// </summary>
        T Generate();

        /// <summary>
        /// Generate multiple instances
        /// </summary>
        List<T> GenerateMultiple(int count);
        
        /// <summary>
        /// Seed the generator for deterministic results
        /// </summary>
        void SetSeed(int seed);
    }
}
