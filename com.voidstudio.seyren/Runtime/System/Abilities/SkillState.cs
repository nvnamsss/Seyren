using System;
using UnityEngine;

namespace Seyren.Abilities
{
    /// <summary>
    /// Interface for different skill states
    /// </summary>
    public interface ISkillState
    {
        /// <summary>
        /// Attempt to unlock the skill
        /// </summary>
        /// <returns>True if the unlock was successful</returns>
        bool TryUnlock();
        
        /// <summary>
        /// Check if the skill can be unlocked
        /// </summary>
        /// <returns>True if the skill can be unlocked</returns>
        bool CanUnlock();
        
        /// <summary>
        /// Get the display name for this state
        /// </summary>
        string GetStateName();
        
        /// <summary>
        /// Process a notification that a prerequisite skill has been unlocked
        /// </summary>
        /// <param name="prerequisiteId">ID of the prerequisite that was unlocked</param>
        void OnPrerequisiteUnlocked(string prerequisiteId);
    }
    
    /// <summary>
    /// Represents the locked state of a skill (prerequisites not met)
    /// </summary>
    public class LockedSkillState : ISkillState
    {
        private readonly SkillNode skill;
        private string _name = "Locked";
        
        public LockedSkillState(SkillNode skill)
        {
            this.skill = skill;
        }
        
        public bool TryUnlock()
        {
            // Cannot unlock while in locked state
            return false;
        }
        
        public bool CanUnlock()
        {
            return false;
        }
        
        public string GetStateName()
        {
            return _name;
        }
        
        public void OnPrerequisiteUnlocked(string prerequisiteId)
        {
            // Mark prerequisite as met
            skill.SetPrerequisiteMet(prerequisiteId);
            
            // Check if all prerequisites are now met, if so transition to Unlockable state
            if (skill.IsUnlockable())
            {
                skill.TransitionToState(new UnlockableSkillState(skill));
            }
        }
    }
    
    /// <summary>
    /// Represents the unlockable state of a skill (prerequisites met but skill not unlocked yet)
    /// </summary>
    public class UnlockableSkillState : ISkillState
    {
        private readonly SkillNode skill;
        
        public UnlockableSkillState(SkillNode skill)
        {
            this.skill = skill;
        }
        
        public bool TryUnlock()
        {
            // Check if all unlock conditions are satisfied
            if (skill.CheckUnlockConditions())
            {
                skill.TransitionToState(new UnlockedSkillState(skill));
                return true;
            }
            return false;
        }
        
        public bool CanUnlock()
        {
            return skill.CheckUnlockConditions();
        }
        
        public string GetStateName()
        {
            return "Unlockable";
        }
        
        public void OnPrerequisiteUnlocked(string prerequisiteId)
        {
            // Already unlockable, nothing to do here
        }
    }
    
    /// <summary>
    /// Represents the unlocked state of a skill
    /// </summary>
    public class UnlockedSkillState : ISkillState
    {
        private readonly SkillNode skill;
        
        public UnlockedSkillState(SkillNode skill)
        {
            this.skill = skill;
        }
        
        public bool TryUnlock()
        {
            // Already unlocked
            return true;
        }
        
        public bool CanUnlock()
        {
            return false; // Already unlocked
        }
        
        public string GetStateName()
        {
            return "Unlocked";
        }
        
        public void OnPrerequisiteUnlocked(string prerequisiteId)
        {
            // Already unlocked, nothing to do here
        }
    }
}