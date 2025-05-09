using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.Abilities
{
    /// <summary>
    /// Represents a single skill in the skill tree
    /// </summary>
    [Serializable]
    public class Skill
    {
        public string skillId;
        public string displayName;
        public string description;
        public Sprite icon;
        public int unlockCost;
        public List<string> prerequisiteSkillIds = new List<string>();
        public bool isUnlocked;

        public Skill(string id, string name, string desc, int cost)
        {
            skillId = id;
            displayName = name;
            description = desc;
            unlockCost = cost;
            isUnlocked = false;
        }
    }

    /// <summary>
    /// Manages a collection of skills that can be unlocked by the player
    /// </summary>
    public class SkillTree : MonoBehaviour
    {
        [SerializeField] private List<Skill> skills = new List<Skill>();
        [SerializeField] private int availablePoints;

        private static SkillTree _instance;

        public static SkillTree Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<SkillTree>();
                    if (_instance == null)
                    {
                        Debug.LogWarning("No SkillTree found in scene, creating one.");
                        GameObject go = new GameObject("SkillTree");
                        _instance = go.AddComponent<SkillTree>();
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Add a skill to the skill tree
        /// </summary>
        /// <param name="skill">The skill to add</param>
        public void AddSkill(Skill skill)
        {
            if (!skills.Exists(s => s.skillId == skill.skillId))
            {
                skills.Add(skill);
            }
            else
            {
                Debug.LogWarning($"Skill with ID {skill.skillId} already exists in the skill tree.");
            }
        }

        /// <summary>
        /// Get a skill by its ID
        /// </summary>
        /// <param name="skillId">The ID of the skill to find</param>
        /// <returns>The skill if found, otherwise null</returns>
        public Skill GetSkill(string skillId)
        {
            return skills.Find(s => s.skillId == skillId);
        }

        /// <summary>
        /// Check if a skill can be unlocked
        /// </summary>
        /// <param name="skillId">The ID of the skill to check</param>
        /// <returns>True if the skill can be unlocked</returns>
        public bool CanUnlockSkill(string skillId)
        {
            Skill skill = GetSkill(skillId);
            
            if (skill == null || skill.isUnlocked)
                return false;

            if (availablePoints < skill.unlockCost)
                return false;

            // Check prerequisites
            foreach (string prerequisiteId in skill.prerequisiteSkillIds)
            {
                Skill prerequisite = GetSkill(prerequisiteId);
                if (prerequisite == null || !prerequisite.isUnlocked)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Unlock a skill if possible
        /// </summary>
        /// <param name="skillId">The ID of the skill to unlock</param>
        /// <returns>True if the skill was successfully unlocked</returns>
        public bool UnlockSkill(string skillId)
        {
            if (!CanUnlockSkill(skillId))
                return false;

            Skill skill = GetSkill(skillId);
            availablePoints -= skill.unlockCost;
            skill.isUnlocked = true;

            Debug.Log($"Unlocked skill: {skill.displayName}");
            return true;
        }

        /// <summary>
        /// Add skill points to the available pool
        /// </summary>
        /// <param name="amount">The amount of points to add</param>
        public void AddSkillPoints(int amount)
        {
            availablePoints += amount;
            Debug.Log($"Added {amount} skill points. Total: {availablePoints}");
        }

        /// <summary>
        /// Reset all skills, refunding all points
        /// </summary>
        public void ResetAllSkills()
        {
            int refundedPoints = 0;
            
            foreach (Skill skill in skills)
            {
                if (skill.isUnlocked)
                {
                    refundedPoints += skill.unlockCost;
                    skill.isUnlocked = false;
                }
            }
            
            availablePoints += refundedPoints;
            Debug.Log($"Reset all skills. Refunded {refundedPoints} points. Total: {availablePoints}");
        }

        /// <summary>
        /// Get all skills in the tree
        /// </summary>
        public List<Skill> GetAllSkills()
        {
            return new List<Skill>(skills);
        }

        /// <summary>
        /// Get the number of available skill points
        /// </summary>
        public int AvailablePoints => availablePoints;
    }
}
