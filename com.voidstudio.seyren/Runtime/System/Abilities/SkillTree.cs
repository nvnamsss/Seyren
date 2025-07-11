using System;
using System.Collections.Generic;
using System.Linq;
using Seyren.Payment;
using Seyren.State;
using UnityEngine;

namespace Seyren.Abilities
{
    public interface ISkillNode
    {
        event Action<ISkillNode> OnUnlocked;
        event Action<ISkillNode> OnStateChanged;
        event Action<ISkillNode> OnUpdated;
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
        void AddPrerequisite(string skillId);
        IReadOnlyList<string> PrerequisiteIds { get; }
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


        // hardcoded for now, can be extended to support different costs per skill
        public ICost IncreaseLevelCost => new SimpleCost(SkillPointsResourceId, 1);
        // hardcoded for now, can be extended to support different costs per skill
        public ICost UnlockCost => new SimpleCost(SkillPointsResourceId, 1);

        public IReadOnlyList<string> PrerequisiteIds => prerequisites.AsReadOnly();

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

        public SkillNode(string id, string name, int level, bool unlockable = false)
        {
            this.id = id;
            this.name = name;
            this.level = level;
            this.prerequisiteStatus = new Dictionary<string, bool>();
            this.prerequisites = new List<string>();

            // Start in the Locked state by default
            this.currentState = unlockable
                ? new UnlockableSkillState(this)
                : new LockedSkillState(this);
            // OnStateChanged += onStateChanged;
        }   

        public ISkillNode WithUnlockCondition(IUnlockCondition condition)
        {
            this.unlockCondition = condition;
            return this;
        }
        
        public void AddPrerequisite(string skillId)
        {
            if (prerequisiteStatus.ContainsKey(skillId))
            {
                Debug.LogWarning($"Prerequisite {skillId} already exists for skill {this.id}");
                return;
            }

            prerequisiteStatus.Add(skillId, false); // Initialize prerequisite status
            prerequisites.Add(skillId);
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

        // private void onStateChanged(ISkillNode skill)
        // {
        //     if (!skill.IsUnlocked)
        //     {
        //         // If the skill is not unlocked, we should not notify dependent skills
        //         return;
        //     }

        //     for (int i = 0; i < dependentSkills.Count; i++)
        //     {
        //         dependentSkills[i].UnlockPrerequisite(skill.Id);
        //     }
        // }
    }
    /// <summary>
    /// Manages a collection of skills and their dependencies
    /// </summary>
    public class SkillTree
    {
        private Dictionary<string, ISkillNode> skills = new Dictionary<string, ISkillNode>();
        private IUnlockContext unlockContext;
        public event Action<ISkillNode> OnSkillUnlocked;
        // Store edges from a node to its children
        // This can be used to traverse the tree or find dependencies
        private Dictionary<string, List<ISkillNode>> edges = new Dictionary<string, List<ISkillNode>>();

        public SkillTree(List<ISkillNode> initialSkills)
        {
            foreach (ISkillNode skill in initialSkills)
            {
                AddSkill(skill);
            }
        }

        public SkillTree WithContext(IUnlockContext context)
        {
            this.unlockContext = context;
            return this;
        }

        public void Initialize()
        {
            // iterate through all nodes, build the tree structure
            foreach (ISkillNode skill in skills.Values)
            {
                // If the skill has prerequisites, add them to the edges
                foreach (string prerequisiteId in skill.PrerequisiteIds)
                {
                    if (skills.TryGetValue(prerequisiteId, out ISkillNode prerequisiteSkill))
                    {
                        if (!edges.ContainsKey(prerequisiteId))
                        {
                            edges[prerequisiteId] = new List<ISkillNode>();
                        }
                        edges[prerequisiteId].Add(skill);
                        // register the event
                        prerequisiteSkill.OnStateChanged += onSkillNodeUnlocked;
                    }
                    else
                    {
                        Debug.LogWarning($"Prerequisite skill {prerequisiteId} for skill {skill.Id} not found in the skill tree");
                    }
                }
            }
        }

        /// <summary>
        /// Add a skill to the skill tree
        /// </summary>
        public void AddSkill(ISkillNode skill)
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
                if (!skill.IsUnlockable)
                {
                    return false;
                }
                
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



        private void onSkillNodeUnlocked(ISkillNode skill)
        {
            if (!skill.IsUnlocked)
            {
                return;
            }

            // Notify all dependent skills that this skill has been unlocked
            if (edges.TryGetValue(skill.Id, out List<ISkillNode> dependents))
            {
                foreach (var dependent in dependents)
                {
                    dependent.UnlockPrerequisite(skill.Id);
                }
            }
        }
    }
}
