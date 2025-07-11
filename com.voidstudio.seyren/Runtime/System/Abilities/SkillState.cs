using System;
using Seyren.State;

namespace Seyren.Abilities
{
    /// <summary>
    /// Represents the locked state of a skill (prerequisites not met)
    /// </summary>
    public class LockedSkillState : IState
    {
        private readonly ISkillNode skill;
        private string _name = "Locked";

        public string ID => _name;

        public LockedSkillState(ISkillNode skill)
        {
            this.skill = skill;
        }

        public void Enter()
        {
        }

        public void Update()
        {
        }

        public void Exit()
        {
        }

        public bool CanTransitionTo(IState nextState)
        {
            if (nextState is UnlockedSkillState || nextState is UnlockableSkillState)
            {
                // Transition to unlocked or unlockable state if conditions are met
                return true;
            }
            return false;
        }
    }
    
    /// <summary>
    /// Represents the unlockable state of a skill (prerequisites met but skill not unlocked yet)
    /// </summary>
    public class UnlockableSkillState : IState
    {
        private readonly ISkillNode skill;
        private string _name = "Unlockable";

        public UnlockableSkillState(ISkillNode skill)
        {
            this.skill = skill;
        }

        public string ID => _name;

        public bool CanTransitionTo(IState nextState)
        {
            if (nextState is UnlockedSkillState)
            {
                // Can transition to unlocked state if the player activates the skill
                return true;
            }
            return false;
        }

        public void Enter()
        {
            // Logic for entering unlockable state
            // For example, highlight the skill as available for unlocking
        }

        public void Exit()
        {
            // Logic for exiting unlockable state
        }

        public void Update()
        {
            // Check for any runtime conditions that might be relevant
            // for an unlockable skill (e.g., player proximity, etc.)
        }
    }
    
    /// <summary>
    /// Represents the unlocked state of a skill
    /// </summary>
    public class UnlockedSkillState : IState
    {
        private readonly SkillNode skill;
        
        public UnlockedSkillState(SkillNode skill)
        {
            this.skill = skill;
        }

        public string ID => "Unlocked";

        public bool CanTransitionTo(IState nextState)
        {
            return false; // Cannot transition from unlocked state
        }

        public void Enter()
        {
            // Logic for entering the unlocked state
        }

        public void Exit()
        {
            // Logic for exiting the unlocked state
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}