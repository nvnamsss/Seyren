using System;
using System.Collections.Generic;
using System.Linq;
using PlasticPipe.PlasticProtocol.Messages;
using Seyren.Payment;
using Seyren.State;
using UnityEngine;

namespace Seyren.Abilities
{
    public interface ISkillNode
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        int Level { get; }

        bool IsUnlocked { get; }
        bool IsUnlockable { get; }
        void IncreaseLevel();
        void DecreaseLevel();
        bool Unlock(IUnlockContext ctx);
        void UnlockPrerequisite(string id);
        event Action<ISkillNode> OnUnlocked;
        event Action<ISkillNode> OnStateChanged;
        event Action<ISkillNode> OnUpdated;
        List<ISkillNode> Childrens { get; }
        ICost IncreaseLevelCost { get; }
        ICost UnlockCost { get; }
    }
    
    /// <summary>
    /// Represents a single skill in the skill tree
    /// </summary>
    public class SkillNode : ISkillNode
    {
        public static string SkillPointsResourceId = "SkillPoints";
        public bool IsUnlocked => currentState is UnlockedSkillState;
        public bool IsUnlockable => currentState is UnlockableSkillState;

        public string Id => id;
        private string id;

        public string Name => name;
        private string name;

        public string Description => description;
        private string description;

        public int Level => level;
        private int level;

        public List<ISkillNode> Childrens => dependentSkills;

        // hardcoded for now, can be extended to support different costs per skill
        public ICost IncreaseLevelCost => new SimpleCost(SkillPointsResourceId, 1);
        // hardcoded for now, can be extended to support different costs per skill
        public ICost UnlockCost => new SimpleCost(SkillPointsResourceId, 1);

        public event Action<ISkillNode> OnUnlocked;
        public event Action<ISkillNode> OnStateChanged;
        public event Action<ISkillNode> OnUpdated;

        private List<string> prerequisites;
        private Dictionary<string, bool> prerequisiteStatus;

        // List of skills that require this skill as a prerequisite
        public List<ISkillNode> dependentSkills = new List<ISkillNode>();

        // State pattern implementation
        private IState currentState;

        // Strategy pattern implementation
        private IUnlockCondition unlockCondition;

        public SkillNode(string id, string name, int level, List<string> prerequisites = null)
        {
            this.id = id;
            this.name = name;
            this.level = level;
            this.prerequisites = prerequisites ?? new List<string>();
            this.prerequisiteStatus = new Dictionary<string, bool>();

            foreach (var prereq in this.prerequisites)
            {
                prerequisiteStatus[prereq] = false; // Initialize all prerequisites as not met
            }

            // Initialize with default unlock condition (no additional conditions)
            this.unlockCondition = null;

            // Start in the Locked state by default
            this.currentState = prerequisites.Count == 0
                ? (IState)new UnlockableSkillState(this)
                : new LockedSkillState(this);
            OnStateChanged += onStateChanged;
        }   

        public void AddChildSkill(ISkillNode childSkill)
        {
            if (dependentSkills.Contains(childSkill))
            {
                Debug.LogWarning($"Child skill {childSkill.Id} already exists for skill {this.id}");
                return;
            }

            dependentSkills.Add(childSkill);
        }

        /// <summary>
        /// Check if all unlock conditions are satisfied
        /// </summary>
        public bool CheckUnlockConditions()
        {
            // If there's no specific condition, default to true
            return unlockCondition == null || unlockCondition.IsSatisfied(null);
        }

        public bool Unlock(IUnlockContext ctx)
        {
            if (currentState is LockedSkillState)
            {
                return false;
            }

            TransitionToState(new UnlockedSkillState(this));
            OnUnlocked?.Invoke(this);
            return true;
        }

        /// <summary>
        /// Mark a prerequisite as met
        /// </summary>
        public void UnlockPrerequisite(string prerequisiteId)
        {
            if (prerequisiteStatus.ContainsKey(prerequisiteId))
            {
                prerequisiteStatus[prerequisiteId] = true;
                // Delegate to the current state to handle the prerequisite being unlocked
                // currentState.OnPrerequisiteUnlocked(prerequisiteId);
            }

            bool allPrerequisitesMet = true;
            for (int i = 0; i < prerequisites.Count; i++)
            {
                if (!prerequisiteStatus.ContainsKey(prerequisites[i]) || !prerequisiteStatus[prerequisites[i]])
                {
                    allPrerequisitesMet = false;
                    break;
                }
            }

            if (!allPrerequisitesMet)
            {
                return;
            }

            TransitionToState(new UnlockableSkillState(this));
        }

        /// <summary>
        /// Transition to a new state (State Pattern)
        /// </summary>
        internal void TransitionToState(IState newState)
        {
            if (!currentState.CanTransitionTo(newState))
            {
                return;
            }

            currentState = newState;
            OnStateChanged?.Invoke(this);
        }


        public void IncreaseLevel()
        {
            if (currentState is LockedSkillState)
            {
                Debug.LogWarning($"Cannot increase level of locked skill {id}");
                return;
            }

            level++;
            // Notify listeners that the skill has been updated
            OnUpdated?.Invoke(this);
        }

        public void DecreaseLevel()
        {
            if (currentState is LockedSkillState)
            {
                Debug.LogWarning($"Cannot decrease level of locked skill {id}");
                return;
            }

            level--;
            OnUpdated?.Invoke(this);
        }

        private void onStateChanged(ISkillNode skill)
        {
            if (!skill.IsUnlocked)
            {
                // If the skill is not unlocked, we should not notify dependent skills
                return;
            }

            for (int i = 0; i < dependentSkills.Count; i++)
            {
                dependentSkills[i].UnlockPrerequisite(skill.Id);
            }
        }
    }
    /// <summary>
    /// Manages a collection of skills and their dependencies
    /// </summary>
    public class SkillTree
    {
        private Dictionary<string, ISkillNode> skills = new Dictionary<string, ISkillNode>();
        private IUnlockContext unlockContext;
        private ISkillNode root;
        public event Action<ISkillNode> OnSkillUnlocked;

        public SkillTree(ISkillNode root)
        {
            this.root = root;
            Initialize();
        }

        private void Initialize()
        {
            // iterate through all root using BFS
            Queue<ISkillNode> queue = new Queue<ISkillNode>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                ISkillNode current = queue.Dequeue();
                AddSkill(current);

                // Enqueue all dependent skills
                List<ISkillNode> dependentSkills = current.Childrens;
                for (int i = 0; i < dependentSkills.Count; i++)
                {
                    queue.Enqueue(dependentSkills[i]);
                }
            }

        }

        /// <summary>
        /// Get a skill by its ID
        /// </summary>
        public ISkillNode GetSkill(string skillId)
        {
            if (skills.TryGetValue(skillId, out ISkillNode skill))
            {
                return skill;
            }
            return null;
        }

        /// <summary>
        /// Unlock a skill by its ID
        /// </summary>
        public bool UnlockSkill(string skillId)
        {
            if (skills.TryGetValue(skillId, out ISkillNode skill))
            {
                bool success = unlockContext.PaymentProcessor.ProcessPayment(cost: skill.UnlockCost);
                if (!success)
                {
                    Debug.LogWarning($"Failed to unlock skill {skillId}: insufficient resources");
                    return false;
                }

                return skill.Unlock(unlockContext);
            }
            Debug.LogWarning($"Skill with ID {skillId} not found");
            return false;
        }


        public void IncreaseLevel(string skillId)
        {
            if (skills.TryGetValue(skillId, out ISkillNode skill))
            {
                bool success = unlockContext.PaymentProcessor.ProcessPayment(cost: skill.IncreaseLevelCost);
                if (!success)
                {
                    Debug.LogWarning($"Failed to increase level of skill {skillId}: insufficient resources");
                    return;
                }

                skill.IncreaseLevel();
            }
            else
            {
                Debug.LogWarning($"Skill with ID {skillId} not found");
            }
        }

        public void DecreaseLevel(string skillId)
        {
            if (skills.TryGetValue(skillId, out ISkillNode skill))
            {
                unlockContext.PaymentProcessor.RefundPayment(cost: skill.IncreaseLevelCost);
                skill.DecreaseLevel();
            }
            else
            {
                Debug.LogWarning($"Skill with ID {skillId} not found");
            }
        }


        /// <summary>
        /// Get all skills in the tree
        /// </summary>
        public IEnumerable<ISkillNode> GetAllSkills()
        {
            return skills.Values;
        }

        /// <summary>
        /// Get all unlocked skills
        /// </summary>
        public IEnumerable<ISkillNode> GetUnlockedSkills()
        {
            return skills.Values.Where(s => s.IsUnlocked);
        }

        /// <summary>
        /// Get all skills that can be unlocked (prerequisites are met but not yet unlocked)
        /// </summary>
        public IEnumerable<ISkillNode> GetAvailableSkills()
        {
            return skills.Values.Where(s => !s.IsUnlocked);
        }

        /// <summary>
        /// Internal method to handle skill unlock events
        /// </summary>
        private void OnSkillUnlockedInternal(ISkillNode skill)
        {
            OnSkillUnlocked?.Invoke(skill);
        }

        /// <summary>
        /// Add a skill to the skill tree
        /// </summary>
        private void AddSkill(ISkillNode skill)
        {
            if (!skills.ContainsKey(skill.Id))
            {
                skills.Add(skill.Id, skill);
                skill.OnUnlocked += OnSkillUnlockedInternal;
            }
            else
            {
                Debug.LogWarning($"Skill with ID {skill.Id} already exists in the skill tree");
            }
        }
    }
}
