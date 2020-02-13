using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.QuestSystem
{
    public class HierarchyCondition
    {
        /* Its called hierarchy thus must have tree in there
         * and of course its non-balance
         * due to this tree is preferred performance in query (For real-time)
         * thus Unlock must be O(1), its mean when a node in tree is modify, all related branch must update
         * and of course data will be stored to ensure unlock take O(1).
         * So time complexity will be O(1) and memory complexity will be O(n)
         * Should it easily to balance depend on Size??? Seem no need
         * Should I apply balance to this tree?
         */
        //ancestor
        //descendant
        /// <summary>
        /// Demetermine if this condition is satisfied
        /// </summary>
        public bool Unlocked { get; }
        public bool IsAncenstor(HierarchyCondition condition)
        {
            if (Ancestor.Count == 0)
            {
                return false;
            }

            if (Ancestor.Contains(condition))
            {
                return true;
            }

            for (int loop = 0; loop < Ancestor.Count; loop++)
            {
                if (Ancestor[loop].IsAncenstor(condition))
                {
                    return true;
                }
            }

            return false;
        }
        protected List<HierarchyCondition> Ancestor;
        /// <summary>
        /// mark added condition as descendant of current condition
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool AddCondition(HierarchyCondition condition)
        {
            if (IsAncenstor(condition))
            {
                return false;
            }

            Ancestor.Add(condition);
            return true;
        }

        public bool Unlock()
        {
            return true;
        }
    }
}
