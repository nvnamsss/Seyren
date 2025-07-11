using System.Collections.Generic;
using NUnit.Framework;
using Seyren.Abilities;
using Seyren.Payment;

namespace Seyren.Tests.Abilities
{
    public class SkillTreeTest
    {
        private SkillTree skillTree;
        private MockUnlockContext ctx;
        private IPaymentProcessor paymentProcessor;
        private IResourceManager resourceManager;


        [SetUp]
        public void Setup()
        {
            resourceManager = new DefaultResourceManager();

            paymentProcessor = new StandardPaymentProcessor(resourceManager);
            ctx = new MockUnlockContext(paymentProcessor);

            // Create a simple skill tree structure
            // Root skill with no prerequisites
            SkillNode rootSkill = new SkillNode("root", "Root Skill", 1, true);

            // First level children
            SkillNode skillA = new SkillNode("skillA", "Skill A", 1);
            SkillNode skillB = new SkillNode("skillB", "Skill B", 1);

            // Second level child (depends on both skillA and skillB)
            SkillNode skillC = new SkillNode("skillC", "Skill C", 1);

            // Connect parent-child relationships
            // rootSkill.AddChildSkill(skillA);
            // rootSkill.AddChildSkill(skillB);
            // skillA.AddChildSkill(skillC);
            // skillB.AddChildSkill(skillC);
            skillC.AddPrerequisite(skillA.Id);
            skillC.AddPrerequisite(skillB.Id);
            skillA.AddPrerequisite(rootSkill.Id);
            skillB.AddPrerequisite(rootSkill.Id);
            
            List<ISkillNode> skills = new List<ISkillNode>
            {
                rootSkill,
                skillA,
                skillB,
                skillC
            };

            // Create the skill tree
            skillTree = new SkillTree(skills).WithContext(ctx);
            skillTree.Initialize();
            // provide resource
            resourceManager.AddResource(SkillNode.SkillPointsResourceId, 10);
        }
        
        [Test]
        public void RootSkillShouldBeUnlockable()
        {
            ISkillNode rootSkill = skillTree.GetSkill("root");
            Assert.IsNotNull(rootSkill);
            Assert.IsTrue(rootSkill.IsUnlockable, "Root skill should be unlockable immediately");
            Assert.IsFalse(rootSkill.IsUnlocked, "Root skill should not be unlocked initially");
        }
        
        [Test]
        public void ShouldUnlockRootSkill()
        {
            // Initial state check
            ISkillNode rootSkill = skillTree.GetSkill("root");
            Assert.IsFalse(rootSkill.IsUnlocked);
            
            // Unlock the root skill
            bool result = skillTree.UnlockSkill("root");
            
            // Verifications
            Assert.IsTrue(result, "Unlock should succeed");
            Assert.IsTrue(rootSkill.IsUnlocked, "Root skill should be unlocked");
            
            int pointLeft = resourceManager.GetResourceAmount(SkillNode.SkillPointsResourceId);
            Assert.AreEqual(9, pointLeft, "Should spend 1 skill point");
        }
        
        [Test]
        public void ShouldMakeDependentSkillsUnlockableWhenPrerequisiteIsUnlocked()
        {
            // Unlock root skill first
            skillTree.UnlockSkill("root");
            
            // Check if dependent skills are now unlockable
            ISkillNode skillA = skillTree.GetSkill("skillA");
            ISkillNode skillB = skillTree.GetSkill("skillB");
            
            Assert.IsTrue(skillA.IsUnlockable, "Skill A should be unlockable after root is unlocked");
            Assert.IsTrue(skillB.IsUnlockable, "Skill B should be unlockable after root is unlocked");
        }
        
        [Test]
        public void ShouldNotUnlockSkillWithLockedPrerequisites()
        {
            // Try to unlock skill A without unlocking root first
            bool result = skillTree.UnlockSkill("skillA");
            
            // Verification
            Assert.IsFalse(result, "Should not be able to unlock skill with locked prerequisites");
            Assert.IsFalse(skillTree.GetSkill("skillA").IsUnlocked);
            int pointLeft = resourceManager.GetResourceAmount(SkillNode.SkillPointsResourceId);
            Assert.AreEqual(10, pointLeft, "Should not spend skill points when unlock fails");
        }
        
        [Test]
        public void ShouldRequireAllPrerequisitesToUnlockSkill()
        {
            // Unlock root
            skillTree.UnlockSkill("root");
            
            // Unlock skill A
            skillTree.UnlockSkill("skillA");
            
            // Try to unlock skill C (should fail because skill B is not unlocked)
            bool result = skillTree.UnlockSkill("skillC");
            Assert.IsFalse(result);
            
            // Unlock skill B
            skillTree.UnlockSkill("skillB");
            
            // Now try to unlock skill C again
            result = skillTree.UnlockSkill("skillC");
            Assert.IsTrue(result, "Should unlock skill C after all prerequisites are met");
            Assert.IsTrue(skillTree.GetSkill("skillC").IsUnlocked);
        }
        
        [Test]
        public void ShouldIncreaseLevelOfUnlockedSkill()
        {
            // Unlock root skill
            skillTree.UnlockSkill("root");
            
            // Initial level check
            ISkillNode rootSkill = skillTree.GetSkill("root");
            int initialLevel = rootSkill.Level;
            
            // Increase level
            skillTree.IncreaseLevel("root");
            
            // Verification
            Assert.AreEqual(initialLevel + 1, rootSkill.Level, "Skill level should increase by 1");
            int pointLeft = resourceManager.GetResourceAmount(SkillNode.SkillPointsResourceId);
            Assert.AreEqual(8, pointLeft, "Should spend 1 skill point for level up");
        }
        
        [Test]
        public void ShouldNotIncreaseLevelIfInsufficientResources()
        {
            // // Unlock root skill
            // skillTree.UnlockSkill("root");
            
            // // Spend all remaining skill points
            // paymentProcessor.SetResourceAmount(SkillNode.SkillPointsResourceId, 0);
            
            // // Get initial level
            // ISkillNode rootSkill = skillTree.GetSkill("root");
            // int initialLevel = rootSkill.Level;
            
            // // Try to increase level
            // skillTree.IncreaseLevel("root");
            
            // // Verification
            // Assert.AreEqual(initialLevel, rootSkill.Level, "Skill level should not change when resources are insufficient");
        }
        
        [Test]
        public void ShouldDecreaseLevelAndRefundPoints()
        {
            // // Unlock root skill
            // skillTree.UnlockSkill("root");
            
            // // Increase level
            // skillTree.IncreaseLevel("root");
            
            // // Get current level and points
            // ISkillNode rootSkill = skillTree.GetSkill("root");
            // int levelAfterIncrease = rootSkill.Level;
            // int pointsAfterIncrease = paymentProcessor.GetResourceAmount(SkillNode.SkillPointsResourceId);
            
            // // Decrease level
            // skillTree.DecreaseLevel("root");
            
            // // Verification
            // Assert.AreEqual(levelAfterIncrease - 1, rootSkill.Level, "Skill level should decrease by 1");
            // Assert.AreEqual(pointsAfterIncrease + 1, paymentProcessor.GetResourceAmount(SkillNode.SkillPointsResourceId), 
            //     "Should refund 1 skill point");
        }
        
        [Test]
        public void ShouldGetAllSkills()
        {
            int skillCount = 0;
            foreach (var skill in skillTree.GetAllSkills())
            {
                skillCount++;
            }
            
            Assert.AreEqual(4, skillCount, "Should have 4 skills in total");
        }
        
        [Test]
        public void ShouldGetUnlockedSkills()
        {
            // Initially no skills are unlocked
            Assert.AreEqual(0, CountSkills(skillTree.GetUnlockedSkills()));
            
            // Unlock some skills
            skillTree.UnlockSkill("root");
            skillTree.UnlockSkill("skillA");
            
            // Verification
            Assert.AreEqual(2, CountSkills(skillTree.GetUnlockedSkills()), "Should have 2 unlocked skills");
        }

        [Test]
        public void ShouldUnlockable()
        {
            skillTree.UnlockSkill("root");
            ISkillNode skillA = skillTree.GetSkill("skillA");
            ISkillNode skillB = skillTree.GetSkill("skillB");
            ISkillNode skillC = skillTree.GetSkill("skillC");

            Assert.IsTrue(skillA.IsUnlockable, "Skill A should be unlockable after root is unlocked");
            Assert.IsTrue(skillB.IsUnlockable, "Skill B should be unlockable after root is unlocked");
            Assert.IsFalse(skillC.IsUnlockable, "Skill C should not be unlockable until both prerequisites are unlocked");

            skillTree.UnlockSkill("skillA");
            Assert.IsFalse(skillC.IsUnlockable, "Skill C should still not be unlockable until skill B is unlocked");
            skillTree.UnlockSkill("skillB");
            Assert.IsTrue(skillC.IsUnlockable, "Skill C should be unlockable after both prerequisites are unlocked");
        }

        
        private int CountSkills(IEnumerable<ISkillNode> skills)
        {
            int count = 0;
            foreach (var skill in skills)
            {
                count++;
            }
            return count;
        }
    }
    
    
    public class MockUnlockContext : IUnlockContext
    {
        public IPaymentProcessor PaymentProcessor { get; }
        private SkillTree skillTreeInstance;
        private Dictionary<string, bool> unlockedSkills = new Dictionary<string, bool>();
        private Dictionary<string, int> resourceValues = new Dictionary<string, int>();
        private int unitLevel = 1;

        public SkillTree SkillTree => skillTreeInstance;

        public int UnitLevel => unitLevel;

        public MockUnlockContext(IPaymentProcessor paymentProcessor)
        {
            PaymentProcessor = paymentProcessor;
        }
        
        public void SetSkillTree(SkillTree skillTree)
        {
            skillTreeInstance = skillTree;
        }
        
        public void SetUnitLevel(int level)
        {
            unitLevel = level;
        }
        
        public void SetResourceValue(string resourceName, int value)
        {
            resourceValues[resourceName] = value;
        }
        
        public void SetSkillUnlocked(string skillId, bool unlocked)
        {
            unlockedSkills[skillId] = unlocked;
        }

        public int GetResourceValue(string resourceName)
        {
            return resourceValues.TryGetValue(resourceName, out int value) ? value : 0;
        }

        public bool IsSkillUnlocked(string skillId)
        {
            if (skillTreeInstance != null)
            {
                var skill = skillTreeInstance.GetSkill(skillId);
                if (skill != null)
                {
                    return skill.IsUnlocked;
                }
            }
            
            return unlockedSkills.TryGetValue(skillId, out bool unlocked) && unlocked;
        }
    }
}
