using Seyren.System.Units;
using UnityEngine;

namespace Seyren.System.Abilities
{
    public abstract class AbilityInstance
    {
        public IUnit caster;
                /// <summary>
        /// Target position if applicable
        /// </summary>
        public Vector3? location;
        
        /// <summary>
        /// Target unit if applicable
        /// </summary>
        public IUnit targetUnit;
    }
}