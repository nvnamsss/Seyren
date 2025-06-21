using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Seyren.Abilities
{
    /// <summary>
    /// Represents a single skill in the skill tree
    /// </summary>
    public class Skill
    {
        public string id;
        public string name;
        public int level;
        public bool IsUnlocked => currentState is UnlockedSkillState;
        public string CurrentStateName => currentState.GetStateName();
        public event Action<Skill> OnUnlocked;
        
        private List<string> prerequisites;
        private Dictionary<string, bool> prerequisiteStatus;
        
        // List of skills that require this skill as a prerequisite
        private List<Skill> dependentSkills = new List<Skill>();
        
        // State pattern implementation
        private ISkillState currentState;
        
        // Strategy pattern implementation
        private IUnlockCondition unlockCondition;
        
        public Skill(string id, string name, int level, List<string> prerequisites = null)
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
                ? (ISkillState)new UnlockableSkillState(this) 
                : new LockedSkillState(this);
        }        /// <summary>
        /// Set the unlock condition for this skill (Strategy Pattern)
        /// </summary>
        public void SetUnlockCondition(IUnlockCondition condition)
        {
            this.unlockCondition = condition;
        }
        
        /// <summary>
        /// Check if all unlock conditions are satisfied
        /// </summary>
        public bool CheckUnlockConditions()
        {
            // If there's no specific condition, default to true
            return unlockCondition == null || unlockCondition.IsSatisfied(null);
        }
        
        /// <summary>
        /// Get the description of unlock conditions
        /// </summary>
        public string GetUnlockConditionDescription()
        {
            return unlockCondition?.GetDescription() ?? "No additional conditions";
        }
        
        /// <summary>
        /// Try to unlock the skill (delegates to current state)
        /// </summary>
        public bool TryUnlock()
        {
            return currentState.TryUnlock();
        }
        
        /// <summary>
        /// Check if the skill can be unlocked (delegates to current state)
        /// </summary>
        public bool CanUnlock()
        {
            return currentState.CanUnlock();
        }
        
        /// <summary>
        /// Register this skill as an observer of its prerequisites
        /// </summary>
        public void RegisterWithPrerequisites(Dictionary<string, Skill> skillMap)
        {
            foreach (var prereqId in prerequisites)
            {
                if (skillMap.TryGetValue(prereqId, out Skill prereqSkill))
                {
                    // Register this skill as a dependent of the prerequisite skill
                    prereqSkill.dependentSkills.Add(this);
                    
                    // If the prerequisite is already unlocked, mark it as satisfied
                    if (prereqSkill.IsUnlocked)
                    {
                        SetPrerequisiteMet(prereqId);
                    }
                }
                else
                {
                    Debug.LogWarning($"Prerequisite skill {prereqId} not found for skill {id}");
                }
            }
        }
        
        /// <summary>
        /// Mark a prerequisite as met
        /// </summary>
        public void SetPrerequisiteMet(string prerequisiteId)
        {
            if (prerequisiteStatus.ContainsKey(prerequisiteId))
            {
                prerequisiteStatus[prerequisiteId] = true;
                
                // Delegate to the current state to handle the prerequisite being unlocked
                currentState.OnPrerequisiteUnlocked(prerequisiteId);
            }
        }
        
        /// <summary>
        /// Check if all prerequisites for this skill are met
        /// </summary>
        public bool AreAllPrerequisitesMet()
        {
            // If there are no prerequisites, they are considered met
            if (prerequisites.Count == 0)
                return true;
            
            // Check if all prerequisite skills are marked as met
            return prerequisites.All(prereq => prerequisiteStatus.ContainsKey(prereq) && prerequisiteStatus[prereq]);
        }
        
        /// <summary>
        /// Transition to a new state (State Pattern)
        /// </summary>
        internal void TransitionToState(ISkillState newState)
        {
            currentState = newState;
        }
        
        /// <summary>
        /// Notify observers that this skill has been unlocked
        /// </summary>
        internal void NotifyUnlocked()
        {
            // Notify observers that this skill has been unlocked
            OnUnlocked?.Invoke(this);
            
            // Notify dependent skills that this prerequisite has been met
            NotifyDependents();
        }
        
        /// <summary>
        /// Notify all dependent skills that this skill has been unlocked
        /// </summary>
        private void NotifyDependents()
        {
            foreach (var dependentSkill in dependentSkills)
            {
                dependentSkill.SetPrerequisiteMet(id);
            }
        }
    }    /// <summary>
    /// Manages a collection of skills and their dependencies
    /// </summary>
    public class SkillTree
    {
        private Dictionary<string, Skill> skills = new Dictionary<string, Skill>();
        private IUnlockContext unlockContext;
        public event Action<Skill> OnSkillUnlocked;

        public SkillTree(IUnlockContext context = null)
        {
            this.unlockContext = context;
        }

        /// <summary>
        /// Set the unlock context for checking skill conditions
        /// </summary>
        public void SetUnlockContext(IUnlockContext context)
        {
            this.unlockContext = context;
        }

        /// <summary>
        /// Add a skill to the skill tree
        /// </summary>
        public void AddSkill(Skill skill)
        {
            if (!skills.ContainsKey(skill.id))
            {
                skills.Add(skill.id, skill);
                skill.OnUnlocked += OnSkillUnlockedInternal;
            }
            else
            {
                Debug.LogWarning($"Skill with ID {skill.id} already exists in the skill tree");
            }
        }

        /// <summary>
        /// Initialize the skill tree by connecting prerequisites
        /// </summary>
        public void Initialize()
        {
            foreach (var skill in skills.Values)
            {
                skill.RegisterWithPrerequisites(skills);
            }
        }

        /// <summary>
        /// Unlock a skill by its ID
        /// </summary>
        public bool UnlockSkill(string skillId)
        {
            if (skills.TryGetValue(skillId, out Skill skill))
            {
                return skill.TryUnlock();
            }
            Debug.LogWarning($"Skill with ID {skillId} not found");
            return false;
        }
        
        /// <summary>
        /// Check if a skill can be unlocked
        /// </summary>
        public bool CanUnlock(string skillId)
        {
            if (skills.TryGetValue(skillId, out Skill skill))
            {
                return skill.CanUnlock();
            }
            return false;
        }
        
        /// <summary>
        /// Get a skill by its ID
        /// </summary>
        public Skill GetSkill(string skillId)
        {
            if (skills.TryGetValue(skillId, out Skill skill))
            {
                return skill;
            }
            return null;
        }
        
        /// <summary>
        /// Get all skills in the tree
        /// </summary>
        public IEnumerable<Skill> GetAllSkills()
        {
            return skills.Values;
        }
        
        /// <summary>
        /// Get all unlocked skills
        /// </summary>
        public IEnumerable<Skill> GetUnlockedSkills()
        {
            return skills.Values.Where(s => s.IsUnlocked);
        }
        
        /// <summary>
        /// Get all skills that can be unlocked (prerequisites are met but not yet unlocked)
        /// </summary>
        public IEnumerable<Skill> GetAvailableSkills()
        {
            return skills.Values.Where(s => !s.IsUnlocked && s.CanUnlock());
        }
        
        /// <summary>
        /// Internal method to handle skill unlock events
        /// </summary>
        private void OnSkillUnlockedInternal(Skill skill)
        {
            OnSkillUnlocked?.Invoke(skill);
        }
    }
}
