using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base2D.System.QuestSystem
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
         /// <summary>
         /// Represents Condition as a hierachy, a condtion could be condition for another condition
         /// </summary>
    public class HierarchyCondition
    {
        public delegate void UnlockHandler(HierarchyCondition sender);
        public delegate bool UnlockConditionHandler();
        /// <summary>
        /// Trigger when this condition is satisfied
        /// </summary>
        public event UnlockHandler UnlockSuccess;
        /// <summary>
        /// Determine condition to unlock itself
        /// </summary>
        public UnlockConditionHandler UnlockCondition;
        /// <summary>
        /// Demetermine if this condition is satisfied
        /// </summary>
        public bool Unlocked => unlocked;
        public bool IsAncestor(HierarchyCondition condition)
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
                if (Ancestor[loop].IsAncestor(condition))
                {
                    return true;
                }
            }

            return false;
        }
        protected int Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = value;
                if (_progress == Ancestor.Count)
                {
                    Unlock();
                }
            }
        }
        protected bool unlocked;
        protected List<HierarchyCondition> Ancestor;
        protected List<HierarchyCondition> Descendant;
        private int _progress;
        public HierarchyCondition()
        {
            Progress = 0;
            Ancestor = new List<HierarchyCondition>();
            Descendant = new List<HierarchyCondition>();
            UnlockCondition = () => true;
        }

        /// <summary>
        /// mark added condition as ancestor of current condition
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool AddCondition(HierarchyCondition condition)
        {
            if (IsAncestor(condition))
            {
                return false;
            }

            Ancestor.Add(condition);
            condition.UnlockSuccess += (s) =>
            {
                Progress += 1;
            };

            return true;
        }

        /// <summary>
        /// Unlock this condition, prefered using if current condition is root
        /// </summary>
        /// <returns></returns>
        public bool Unlock()
        {
            if (unlocked || Progress < Ancestor.Count)
            {
                return false;
            }
            
            if (UnlockCondition == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Unlock condition is assigned to null");
#endif
                return false;
            }

            unlocked = true;
            UnlockSuccess?.Invoke(this);
            return true;
        }
    }

    /// <summary>
    /// Represent generic type for 
    /// <see cref="HierarchyCondition"/>
    /// It's will be used as a backbone for
    /// <see cref="TAttactor"/> 
    /// </summary>
    /// <typeparam name="TAttactor"></typeparam>
    public class HierarchyCondition<TAttactor>
    {
        public delegate void UnlockHandler<T>(HierarchyCondition<T> sender);
        public delegate bool UnlockConditionHandler();
        /// <summary>
        /// Trigger when this condition is satisfied
        /// </summary>
        public event UnlockHandler<TAttactor> UnlockSuccess;
        /// <summary>
        /// Determine condition to unlock itself
        /// </summary>
        public UnlockConditionHandler UnlockCondition;
        public TAttactor Attactor { get; }    
        /// <summary>
        /// Demetermine if this condition is satisfied
        /// </summary>
        public bool Unlocked => unlocked;
        public bool IsAncestor(HierarchyCondition<TAttactor> condition)
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
                if (Ancestor[loop].IsAncestor(condition))
                {
                    return true;
                }
            }

            return false;
        }
        protected int Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = value;
                if (_progress == Ancestor.Count)
                {
                    Unlock();
                }
            }
        }
        protected bool unlocked;
        protected List<HierarchyCondition<TAttactor>> Ancestor;
        protected List<HierarchyCondition<TAttactor>> Descendant;
        private int _progress;
        public HierarchyCondition(TAttactor attactor)
        {
            Attactor = attactor;
            Progress = 0;
            Ancestor = new List<HierarchyCondition<TAttactor>>();
            Descendant = new List<HierarchyCondition<TAttactor>>();
            UnlockCondition = () => true;
        }

        /// <summary>
        /// mark added condition as ancestor of current condition
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool AddCondition(HierarchyCondition<TAttactor> condition)
        {
            if (IsAncestor(condition))
            {
                return false;
            }

            Ancestor.Add(condition);
            condition.UnlockSuccess += (s) =>
            {
                Progress += 1;
            };

            return true;
        }

        /// <summary>
        /// Unlock this condition, prefered using if current condition is root
        /// </summary>
        /// <returns></returns>
        public bool Unlock()
        {
            if (unlocked || Progress < Ancestor.Count)
            {
                return false;
            }

            if (UnlockCondition == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Unlock condition is assigned to null");
#endif
                return false;
            }

            unlocked = true;
            UnlockSuccess?.Invoke(this);
            return true;
        }
    }
}
