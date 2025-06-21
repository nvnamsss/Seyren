using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Seyren.Abilities;

namespace Seyren.Tests
{
    public class SkillTreeTest
    {
        private SkillTree skillTree;
        private List<Skill> unlockedSkills;

        [SetUp]
        public void Setup()
        {
            skillTree = new SkillTree();
            unlockedSkills = new List<Skill>();
            skillTree.OnSkillUnlocked += (skill) => unlockedSkills.Add(skill);
        }

        [Test]
        public void BasicSkillUnlocking()
        {
            // Create a basic skill with no prerequisites
            Skill basicSkill = new Skill("basic", "Basic Skill", 1);
            skillTree.AddSkill(basicSkill);
            skillTree.Initialize();

            // Test unlocking the skill
            Assert.IsTrue(skillTree.UnlockSkill("basic"));
            Assert.IsTrue(basicSkill.isUnlocked);
            Assert.AreEqual(1, unlockedSkills.Count);
            Assert.AreEqual("basic", unlockedSkills[0].id);
        }

        [Test]
        public void CannotUnlockNonexistentSkill()
        {
            // Attempt to unlock a skill that doesn't exist
            Assert.IsFalse(skillTree.UnlockSkill("nonexistent"));
            Assert.AreEqual(0, unlockedSkills.Count);
        }

        [Test]
        public void SkillWithPrerequisites()
        {
            // Create a basic skill and an advanced skill that requires the basic skill
            Skill basicSkill = new Skill("basic", "Basic Skill", 1);
            Skill advancedSkill = new Skill("advanced", "Advanced Skill", 2, new List<string> { "basic" });
            
            skillTree.AddSkill(basicSkill);
            skillTree.AddSkill(advancedSkill);
            skillTree.Initialize();

            // Test that advanced skill cannot be unlocked before its prerequisite
            Assert.IsFalse(skillTree.UnlockSkill("advanced"));
            Assert.IsFalse(advancedSkill.isUnlocked);
            Assert.AreEqual(0, unlockedSkills.Count);

            // Unlock the prerequisite
            Assert.IsTrue(skillTree.UnlockSkill("basic"));
            Assert.IsTrue(basicSkill.isUnlocked);
            Assert.AreEqual(1, unlockedSkills.Count);

            // Now advanced skill can be unlocked
            Assert.IsTrue(skillTree.UnlockSkill("advanced"));
            Assert.IsTrue(advancedSkill.isUnlocked);
            Assert.AreEqual(2, unlockedSkills.Count);
        }

        [Test]
        public void AutomaticUnlockingOfDependentSkills()
        {
            // Create a skill tree with a chain of dependencies
            Skill skill1 = new Skill("skill1", "Skill 1", 1);
            Skill skill2 = new Skill("skill2", "Skill 2", 2, new List<string> { "skill1" });
            Skill skill3 = new Skill("skill3", "Skill 3", 3, new List<string> { "skill2" });
            
            skillTree.AddSkill(skill1);
            skillTree.AddSkill(skill2);
            skillTree.AddSkill(skill3);
            skillTree.Initialize();

            // Unlock the first skill, which should only unlock that skill
            skillTree.UnlockSkill("skill1");
            Assert.IsTrue(skill1.isUnlocked);
            Assert.IsFalse(skill2.isUnlocked);
            Assert.IsFalse(skill3.isUnlocked);
            Assert.AreEqual(1, unlockedSkills.Count);

            // Unlock the second skill, which should only unlock that skill
            skillTree.UnlockSkill("skill2");
            Assert.IsTrue(skill1.isUnlocked);
            Assert.IsTrue(skill2.isUnlocked);
            Assert.IsFalse(skill3.isUnlocked);
            Assert.AreEqual(2, unlockedSkills.Count);

            // Unlock the third skill
            skillTree.UnlockSkill("skill3");
            Assert.IsTrue(skill1.isUnlocked);
            Assert.IsTrue(skill2.isUnlocked);
            Assert.IsTrue(skill3.isUnlocked);
            Assert.AreEqual(3, unlockedSkills.Count);
        }

        [Test]
        public void MultiplePrerequisitesTest()
        {
            // Create a skill that requires multiple prerequisites
            Skill skill1 = new Skill("skill1", "Skill 1", 1);
            Skill skill2 = new Skill("skill2", "Skill 2", 1);
            Skill masterSkill = new Skill("master", "Master Skill", 3, new List<string> { "skill1", "skill2" });
            
            skillTree.AddSkill(skill1);
            skillTree.AddSkill(skill2);
            skillTree.AddSkill(masterSkill);
            skillTree.Initialize();

            // Verify master skill cannot be unlocked initially
            Assert.IsFalse(skillTree.UnlockSkill("master"));
            Assert.IsFalse(masterSkill.isUnlocked);

            // Unlock one prerequisite
            skillTree.UnlockSkill("skill1");
            Assert.IsFalse(masterSkill.isUnlocked);

            // Unlock the second prerequisite, which should allow the master skill to be unlocked
            skillTree.UnlockSkill("skill2");
            Assert.IsTrue(skill1.isUnlocked);
            Assert.IsTrue(skill2.isUnlocked);
            Assert.IsFalse(masterSkill.isUnlocked); // Not auto-unlocked yet
            
            // Now we can unlock the master skill
            Assert.IsTrue(skillTree.UnlockSkill("master"));
            Assert.IsTrue(masterSkill.isUnlocked);
            Assert.AreEqual(3, unlockedSkills.Count);
        }

        [Test]
        public void GetAvailableSkillsTest()
        {
            // Create multiple skills with dependencies
            Skill skill1 = new Skill("skill1", "Skill 1", 1);
            Skill skill2 = new Skill("skill2", "Skill 2", 2, new List<string> { "skill1" });
            Skill skill3 = new Skill("skill3", "Skill 3", 3, new List<string> { "skill1", "skill2" });
            
            skillTree.AddSkill(skill1);
            skillTree.AddSkill(skill2);
            skillTree.AddSkill(skill3);
            skillTree.Initialize();

            // Initially, only skill1 should be available
            var availableSkills = skillTree.GetAvailableSkills();
            Assert.AreEqual(1, availableSkills.Count());
            Assert.AreEqual("skill1", availableSkills.First().id);

            // After unlocking skill1, skill2 should become available
            skillTree.UnlockSkill("skill1");
            availableSkills = skillTree.GetAvailableSkills();
            Assert.AreEqual(1, availableSkills.Count());
            Assert.AreEqual("skill2", availableSkills.First().id);

            // After unlocking skill2, skill3 should become available
            skillTree.UnlockSkill("skill2");
            availableSkills = skillTree.GetAvailableSkills();
            Assert.AreEqual(1, availableSkills.Count());
            Assert.AreEqual("skill3", availableSkills.First().id);

            // After unlocking all skills, none should be available
            skillTree.UnlockSkill("skill3");
            availableSkills = skillTree.GetAvailableSkills();
            Assert.AreEqual(0, availableSkills.Count());
        }

        [Test]
        public void CanUnlockTest()
        {
            // Create skills with dependencies
            Skill skill1 = new Skill("skill1", "Skill 1", 1);
            Skill skill2 = new Skill("skill2", "Skill 2", 2, new List<string> { "skill1" });
            
            skillTree.AddSkill(skill1);
            skillTree.AddSkill(skill2);
            skillTree.Initialize();

            // Check initial can unlock status
            Assert.IsTrue(skillTree.CanUnlock("skill1"));
            Assert.IsFalse(skillTree.CanUnlock("skill2"));

            // After unlocking skill1, skill2 should be unlockable
            skillTree.UnlockSkill("skill1");
            Assert.IsFalse(skillTree.CanUnlock("skill1")); // Already unlocked
            Assert.IsTrue(skillTree.CanUnlock("skill2"));

            // After unlocking skill2, nothing should be unlockable
            skillTree.UnlockSkill("skill2");
            Assert.IsFalse(skillTree.CanUnlock("skill1"));
            Assert.IsFalse(skillTree.CanUnlock("skill2"));
        }

        [Test]
        public void RegisterWithPrerequisitesTest()
        {
            // Create a chain of skills with the last one added before its prerequisites
            Skill skill3 = new Skill("skill3", "Skill 3", 3, new List<string> { "skill2" });
            Skill skill2 = new Skill("skill2", "Skill 2", 2, new List<string> { "skill1" });
            Skill skill1 = new Skill("skill1", "Skill 1", 1);
            
            // Add in reverse order to test proper registration
            skillTree.AddSkill(skill3);
            skillTree.AddSkill(skill2);
            skillTree.AddSkill(skill1);
            skillTree.Initialize();
            
            // Test the unlocking chain
            skillTree.UnlockSkill("skill1");
            Assert.IsTrue(skill1.isUnlocked);
            Assert.IsFalse(skill2.isUnlocked);
            
            skillTree.UnlockSkill("skill2");
            Assert.IsTrue(skill2.isUnlocked);
            Assert.IsFalse(skill3.isUnlocked);
            
            skillTree.UnlockSkill("skill3");
            Assert.IsTrue(skill3.isUnlocked);
        }

        [Test]
        public void UnlockedSkillsListTest()
        {
            // Create multiple skills
            Skill skill1 = new Skill("skill1", "Skill 1", 1);
            Skill skill2 = new Skill("skill2", "Skill 2", 2);
            Skill skill3 = new Skill("skill3", "Skill 3", 3);
            
            skillTree.AddSkill(skill1);
            skillTree.AddSkill(skill2);
            skillTree.AddSkill(skill3);
            skillTree.Initialize();

            // Initially no skills should be unlocked
            Assert.AreEqual(0, skillTree.GetUnlockedSkills().Count());

            // Unlock first skill
            skillTree.UnlockSkill("skill1");
            var unlockedSkills = skillTree.GetUnlockedSkills();
            Assert.AreEqual(1, unlockedSkills.Count());
            Assert.AreEqual("skill1", unlockedSkills.First().id);

            // Unlock third skill
            skillTree.UnlockSkill("skill3");
            unlockedSkills = skillTree.GetUnlockedSkills().ToList();
            Assert.AreEqual(2, unlockedSkills.Count());
            Assert.IsTrue(unlockedSkills.Any(s => s.id == "skill1"));
            Assert.IsTrue(unlockedSkills.Any(s => s.id == "skill3"));
        }

        [Test]
        public void AutomaticUnlockForMultiplePrerequisites()
        {
            // Create a tree with auto-unlocking capabilities
            Skill skill1 = new Skill("skill1", "Skill 1", 1);
            Skill skill2 = new Skill("skill2", "Skill 2", 1);
            
            // This skill has both skill1 and skill2 as prerequisites and should auto-unlock
            Skill combinedSkill = new Skill("combined", "Combined Skill", 2, 
                new List<string> { "skill1", "skill2" });
                
            // This skill depends on the combined skill
            Skill finalSkill = new Skill("final", "Final Skill", 3,
                new List<string> { "combined" });
                
            skillTree.AddSkill(skill1);
            skillTree.AddSkill(skill2);
            skillTree.AddSkill(combinedSkill);
            skillTree.AddSkill(finalSkill);
            
            // Implement the auto-unlock feature by always checking dependent skills
            combinedSkill.OnUnlocked += (_) => finalSkill.CheckAndUnlock();
            
            skillTree.Initialize();
            
            // Unlock the first prerequisite
            skillTree.UnlockSkill("skill1");
            Assert.IsTrue(skill1.isUnlocked);
            Assert.IsFalse(combinedSkill.isUnlocked);
            Assert.IsFalse(finalSkill.isUnlocked);
            
            // Unlock the second prerequisite, which should trigger the combined skill
            // to be available for unlocking, but not auto-unlock it
            skillTree.UnlockSkill("skill2");
            Assert.IsTrue(skill2.isUnlocked);
            Assert.IsTrue(skillTree.CanUnlock("combined"));
            Assert.IsFalse(combinedSkill.isUnlocked);
            
            // Now unlock the combined skill
            skillTree.UnlockSkill("combined");
            Assert.IsTrue(combinedSkill.isUnlocked);
            
            // The final skill should now be unlockable
            Assert.IsTrue(skillTree.CanUnlock("final"));
            skillTree.UnlockSkill("final");
            Assert.IsTrue(finalSkill.isUnlocked);
        }
        
        [Test]
        public void SkillTreeComplexDependencyTest()
        {
            /*
             * Create a more complex skill tree:
             *       A
             *     /   \
             *    B     C
             *   / \   / \
             *  D   E F   G
             *   \ /     /
             *    H     /
             *     \   /
             *       I
             */
            
            Skill skillA = new Skill("A", "Skill A", 1);
            
            Skill skillB = new Skill("B", "Skill B", 2, new List<string> { "A" });
            Skill skillC = new Skill("C", "Skill C", 2, new List<string> { "A" });
            
            Skill skillD = new Skill("D", "Skill D", 3, new List<string> { "B" });
            Skill skillE = new Skill("E", "Skill E", 3, new List<string> { "B" });
            Skill skillF = new Skill("F", "Skill F", 3, new List<string> { "C" });
            Skill skillG = new Skill("G", "Skill G", 3, new List<string> { "C" });
            
            Skill skillH = new Skill("H", "Skill H", 4, new List<string> { "D", "E" });
            
            Skill skillI = new Skill("I", "Skill I", 5, new List<string> { "H", "G" });
            
            // Add skills to the tree
            skillTree.AddSkill(skillA);
            skillTree.AddSkill(skillB);
            skillTree.AddSkill(skillC);
            skillTree.AddSkill(skillD);
            skillTree.AddSkill(skillE);
            skillTree.AddSkill(skillF);
            skillTree.AddSkill(skillG);
            skillTree.AddSkill(skillH);
            skillTree.AddSkill(skillI);
            
            skillTree.Initialize();
            
            // Verify initial available skills
            var available = skillTree.GetAvailableSkills().ToList();
            Assert.AreEqual(1, available.Count);
            Assert.AreEqual("A", available[0].id);
            
            // Unlock A, which should make B and C available
            skillTree.UnlockSkill("A");
            available = skillTree.GetAvailableSkills().ToList();
            Assert.AreEqual(2, available.Count);
            Assert.IsTrue(available.Any(s => s.id == "B"));
            Assert.IsTrue(available.Any(s => s.id == "C"));
            
            // Unlock B, which should make D and E available
            skillTree.UnlockSkill("B");
            available = skillTree.GetAvailableSkills().ToList();
            Assert.AreEqual(3, available.Count);
            Assert.IsTrue(available.Any(s => s.id == "C"));
            Assert.IsTrue(available.Any(s => s.id == "D"));
            Assert.IsTrue(available.Any(s => s.id == "E"));
            
            // Unlock D and E, which should make H available
            skillTree.UnlockSkill("D");
            skillTree.UnlockSkill("E");
            available = skillTree.GetAvailableSkills().ToList();
            Assert.AreEqual(2, available.Count);
            Assert.IsTrue(available.Any(s => s.id == "C"));
            Assert.IsTrue(available.Any(s => s.id == "H"));
            
            // Unlock C, which should make F and G available
            skillTree.UnlockSkill("C");
            available = skillTree.GetAvailableSkills().ToList();
            Assert.AreEqual(3, available.Count);
            Assert.IsTrue(available.Any(s => s.id == "F"));
            Assert.IsTrue(available.Any(s => s.id == "G"));
            Assert.IsTrue(available.Any(s => s.id == "H"));
            
            // Unlock H and G, which should make I available
            skillTree.UnlockSkill("H");
            skillTree.UnlockSkill("G");
            available = skillTree.GetAvailableSkills().ToList();
            Assert.AreEqual(2, available.Count);
            Assert.IsTrue(available.Any(s => s.id == "F"));
            Assert.IsTrue(available.Any(s => s.id == "I"));
            
            // Unlock I
            skillTree.UnlockSkill("I");
            available = skillTree.GetAvailableSkills().ToList();
            Assert.AreEqual(1, available.Count);
            Assert.IsTrue(available.Any(s => s.id == "F"));
            
            // Verify the total number of unlocked skills
            var unlocked = skillTree.GetUnlockedSkills().ToList();
            Assert.AreEqual(7, unlocked.Count);
            Assert.IsTrue(unlocked.All(s => s.id != "F"));
        }
    }
}