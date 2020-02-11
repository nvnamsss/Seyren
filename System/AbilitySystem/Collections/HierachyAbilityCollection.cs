using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.AbilitySystem.Collections
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
