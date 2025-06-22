using System.Collections.Generic;
using Seyren.Payment;
using UnityEngine;

namespace Seyren.Abilities
{
    /// <summary>
    /// Interface for skill unlock conditions (Strategy Pattern)
    /// </summary>
    public interface IUnlockCondition
    {
        /// <summary>
        /// Check if the condition is satisfied
        /// </summary>
        /// <returns>True if the condition is met</returns>
        bool IsSatisfied(IUnlockContext context);
        
        /// <summary>
        /// Get a human-readable description of this unlock condition
        /// </summary>
        string GetDescription();
    }
    
    /// <summary>
    /// Context data for unlock conditions
    /// </summary>
    public interface IUnlockContext
    {
        /// <summary>
        /// Get the skill tree
        /// </summary>
        SkillTree SkillTree { get; }
        IPaymentProcessor PaymentProcessor { get; }
        
        /// <summary>
        /// Get the current unit level
        /// </summary>
        int UnitLevel { get; }
        
        /// <summary>
        /// Get resource value by name
        /// </summary>
        int GetResourceValue(string resourceName);
        
        /// <summary>
        /// Check if a skill is unlocked
        /// </summary>
        bool IsSkillUnlocked(string skillId);
    }
    
    /// <summary>
    /// Default implementation of the unlock context
    /// </summary>
    public class DefaultUnlockContext : IUnlockContext
    {
        public SkillTree SkillTree { get; private set; }
        public int UnitLevel { get; private set; }
        private Dictionary<string, int> resources;
        public IPaymentProcessor PaymentProcessor => paymentProcessor;
        private IPaymentProcessor paymentProcessor;
        
        public DefaultUnlockContext(IPaymentProcessor paymentProcessor, SkillTree skillTree, int unitLevel, Dictionary<string, int> resources = null)
        {
            this.paymentProcessor = paymentProcessor;   
            SkillTree = skillTree;
            UnitLevel = unitLevel;
            this.resources = resources ?? new Dictionary<string, int>();
        }
        
        public int GetResourceValue(string resourceName)
        {
            if (resources.TryGetValue(resourceName, out int value))
            {
                return value;
            }
            return 0;
        }
        
        public bool IsSkillUnlocked(string skillId)
        {
            var skill = SkillTree.GetSkill(skillId);
            return skill != null && skill.IsUnlocked;
        }
    }
    
    /// <summary>
    /// Base class for unlock conditions to simplify creation
    /// </summary>
    public abstract class UnlockConditionBase : IUnlockCondition
    {
        public abstract bool IsSatisfied(IUnlockContext context);
        public abstract string GetDescription();
    }
    
    /// <summary>
    /// Check if the unit has reached a specific level
    /// </summary>
    public class LevelUnlockCondition : UnlockConditionBase
    {
        private int requiredLevel;
        
        public LevelUnlockCondition(int requiredLevel)
        {
            this.requiredLevel = requiredLevel;
        }
        
        public override bool IsSatisfied(IUnlockContext context)
        {
            return context.UnitLevel >= requiredLevel;
        }
        
        public override string GetDescription()
        {
            return $"Requires level {requiredLevel}";
        }
    }
    
    /// <summary>
    /// Check if the specified skill is unlocked
    /// </summary>
    public class SkillDependencyCondition : UnlockConditionBase
    {
        private string requiredSkillId;
        private int requiredLevel;

        public SkillDependencyCondition(string requiredSkillId, int requiredLevel = 1)
        {
            this.requiredSkillId = requiredSkillId;
            this.requiredLevel = requiredLevel;
        }
        
        public override bool IsSatisfied(IUnlockContext context)
        {
            return context.IsSkillUnlocked(requiredSkillId) &&
                   context.SkillTree.GetSkill(requiredSkillId).Level >= requiredLevel;
        }
        
        public override string GetDescription()
        {
            return $"Requires skill: {requiredSkillId}";
        }
    }
    
    /// <summary>
    /// Check if a specific resource requirement is met
    /// </summary>
    public class ResourceUnlockCondition : UnlockConditionBase
    {
        private string resourceName;
        private int requiredAmount;
        
        public ResourceUnlockCondition(string resourceName, int requiredAmount)
        {
            this.resourceName = resourceName;
            this.requiredAmount = requiredAmount;
        }
        
        public override bool IsSatisfied(IUnlockContext context)
        {
            return context.GetResourceValue(resourceName) >= requiredAmount;
        }
        
        public override string GetDescription()
        {
            return $"Requires {requiredAmount} {resourceName}";
        }
    }
    
    /// <summary>
    /// Composite condition that requires all child conditions to be met (Chain of Responsibility)
    /// </summary>
    public class CompositeUnlockCondition : UnlockConditionBase
    {
        private List<IUnlockCondition> conditions = new List<IUnlockCondition>();
        
        public CompositeUnlockCondition(params IUnlockCondition[] initialConditions)
        {
            if (initialConditions != null)
            {
                conditions.AddRange(initialConditions);
            }
        }
        
        public void AddCondition(IUnlockCondition condition)
        {
            conditions.Add(condition);
        }
        
        public override bool IsSatisfied(IUnlockContext context)
        {
            foreach (var condition in conditions)
            {
                if (!condition.IsSatisfied(context))
                {
                    return false;
                }
            }
            return true;
        }
        
        public override string GetDescription()
        {
            if (conditions.Count == 0)
                return "No conditions";
                
            string description = "Requires: ";
            for (int i = 0; i < conditions.Count; i++)
            {
                if (i > 0) description += ", ";
                description += conditions[i].GetDescription();
            }
            return description;
        }
    }
}