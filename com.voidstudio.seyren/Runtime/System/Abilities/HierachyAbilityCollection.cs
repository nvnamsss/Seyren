using System.Collections.Generic;

namespace Seyren.System.Abilities.Collections
{
    /// <summary>
    /// Represents AbilityCollection which have Ancestor and Descendant. Suitable for Job tier Ability
    /// </summary>
    public class HierachyAbilityCollection
    {
        public int Count { get; }
        /// <summary>
        /// List Ascentor 
        /// </summary>
        public List<HierachyAbilityCollection> Ancestors;
        public List<HierachyAbilityCollection> Descendants;
    }
}
