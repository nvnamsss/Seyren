using System;
using System.Collections.Generic;

namespace Seyren.System.States
{
    [Serializable]
    public class BaseInt
    {
        public static BaseInt Zero = new BaseInt(0, 0);

        public int Base;
        public int Incr;
        public int Total => Base + Incr;
        public BaseInt(int b, int i)
        {
            Base = b;
            Incr = i;
        }

        public int Amplify(float percent)
        {

            return Total;
        }

        public int Increase(int value)
        {
            return Total;
        }
    }

    [Serializable]
    public class BaseFloat
    {
        public static BaseFloat Zero = new BaseFloat(0, 0);
        public float Base;
        public float Incr => incr;
        public float Total => Base + Incr;
        private Dictionary<string, float> bonuses = new Dictionary<string, float>();
        private object _lock = new object();
        public float this[string key]
        {
            get
            {
                if (bonuses.TryGetValue(key, out float value))
                {
                    return value;
                }
                return 0f;
            }
            set
            {
                bonuses[key] = value;
            }
        }
        private float incr = 0;
        
        /// <summary>
        /// Adds a bonus with the specified key
        /// </summary>
        /// <param name="key">Unique identifier for the bonus</param>
        /// <param name="value">Value of the bonus</param>
        /// <returns>The total value after adding the bonus</returns>
        public float AddBonus(string key, float value)
        {
            bonuses[key] = value;
            CalculateIncr();
            return Total;
        }
        
        /// <summary>
        /// Removes a bonus with the specified key
        /// </summary>
        /// <param name="key">Unique identifier for the bonus</param>
        /// <returns>The total value after removing the bonus</returns>
        public float RemoveBonus(string key)
        {
            if (bonuses.ContainsKey(key))
            {
                bonuses.Remove(key);
                CalculateIncr();
            }
            return Total;
        }
        
        /// <summary>
        /// Check if a bonus exists
        /// </summary>
        /// <param name="key">Unique identifier for the bonus</param>
        /// <returns>True if the bonus exists</returns>
        public bool HasBonus(string key)
        {
            return bonuses.ContainsKey(key);
        }
        
        /// <summary>
        /// Get all bonus keys
        /// </summary>
        /// <returns>Array of all bonus keys</returns>
        public string[] GetBonusKeys()
        {
            string[] keys = new string[bonuses.Count];
            bonuses.Keys.CopyTo(keys, 0);
            return keys;
        }
        
        /// <summary>
        /// Clear all bonuses
        /// </summary>
        /// <returns>The total value after clearing all bonuses</returns>
        public float ClearBonuses()
        {
            bonuses.Clear();
            CalculateIncr();
            return Total;
        }
        
        /// <summary>
        /// Recalculates the total increment value from all bonuses
        /// </summary>
        private void CalculateIncr()
        {
            float totalBonus = 0f;
            foreach (var bonus in bonuses.Values)
            {
                totalBonus += bonus;
            }
            incr = totalBonus;
        }
        
        public BaseFloat(float baseValue) : this(baseValue, 0)
        {

        }

        public BaseFloat(float baseValue, float increasedValue)
        {
            Base = baseValue;
            incr = increasedValue;
        }

        public float Amplify(float percent)
        {
            incr = Base * percent / 100;
            return Total;
        }

        public float Increase(float value)
        {
            incr += value;
            return Total;
        }

        public static BaseFloat operator +(BaseFloat lhs, BaseFloat rhs)
        {
            lhs.Base += rhs.Base;
            lhs.incr += rhs.Incr;
            return lhs;
        }
        public static BaseFloat operator -(BaseFloat lhs, BaseFloat rhs)
        {
            lhs.Base -= rhs.Base;
            lhs.incr -= rhs.Incr;
            return lhs;
        }

        public static BaseFloat operator +(BaseFloat lhs, float rhs)
        {
            lhs.Base += rhs;
            return lhs;
        }

        public static BaseFloat operator -(BaseFloat lhs, float rhs)
        {
            return lhs + (-rhs);
        }


    }

    /// <summary>
    /// Defines standard attribute names used throughout the game systems.
    /// This class provides a centralized location for attribute identifiers to prevent typos and maintain consistency.
    /// </summary>
    /// <remarks>
    /// Use these constants when referring to attributes in your code instead of hard-coding strings.
    /// Some attributes have alternative names (e.g., HEALTH is an alternative for MAX_HP) for better readability.
    /// </remarks>
    public static class AttributeName
    {
        /// <summary>Primary attribute affecting physical damage and health</summary>
        public const string STRENGTH = "Strength";
        
        /// <summary>Primary attribute affecting magical abilities and mana</summary>
        public const string INTELLIGENT = "Intelligent";
        
        /// <summary>Primary attribute affecting speed and precision</summary>
        public const string AGILITY = "Agility";
        
        /// <summary>Base physical attack damage</summary>
        public const string ATTACK_DAMAGE = "AttackDamage";
        
        /// <summary>Percentage increase in magical damage</summary>
        public const string M_DAMAGE_AMPLIFIED = "MDamageAmplified";
        
        /// <summary>Maximum health points</summary>
        public const string MAX_HP = "MaxHp";
        /// <summary>Alternative name for MAX_HP</summary>
        public const string HEALTH = MAX_HP;
        
        /// <summary>Maximum mana points</summary>
        public const string MAX_MP = "MaxMp";
        /// <summary>Alternative name for MAX_MP</summary>
        public const string MANA = MAX_MP;
        
        /// <summary>Current health points</summary>
        public const string CUR_HP = "CurHp";
        
        /// <summary>Current mana points</summary>
        public const string CUR_MP = "CurMp";
        
        /// <summary>Health regeneration per second</summary>
        public const string HP_REGEN = "HpRegen";
        
        /// <summary>Mana regeneration per second</summary>
        public const string MP_REGEN = "MpRegen";
        
        /// <summary>Physical shield regeneration rate</summary>
        public const string SHIELD_REGEN = "ShieldRegen";
        
        /// <summary>Magical shield regeneration rate</summary>
        public const string M_SHIELD_REGEN = "MShieldRegen";
        
        /// <summary>Physical shield value</summary>
        public const string P_SHIELD = "PShield";
        
        /// <summary>Magical shield value</summary>
        public const string M_SHIELD = "MShield";
        
        /// <summary>Percentage of max health regenerated per second</summary>
        public const string HP_REGEN_PERCENT = "HpRegenPercent";
        
        /// <summary>Percentage of max mana regenerated per second</summary>
        public const string MP_REGEN_PERCENT = "MpRegenPercent";
        
        /// <summary>Physical defense reducing incoming physical damage</summary>
        public const string DEFENSE = "Defense";
        
        /// <summary>Magical armor reducing incoming magical damage</summary>
        public const string M_ARMOR = "MArmor";
        
        /// <summary>Maximum distance for basic attacks</summary>
        public const string ATTACK_RANGE = "AttackRange";
        
        /// <summary>Maximum distance for ability casting</summary>
        public const string CAST_RANGE = "CastRange";
        
        /// <summary>Base movement speed</summary>
        public const string MOVEMENT_SPEED = "MovementSpeed";
        
        /// <summary>Attack speed modifier affecting attack animation speed</summary>
        public const string ATTACK_SPEED = "AttackSpeed";
        
        /// <summary>Speed at which the unit can change direction</summary>
        public const string TURN_RATE = "TurnRate";
        
        /// <summary>Current level of the unit</summary>
        public const string LEVEL = "Level";
    }
    
    public interface IAttribute
    {
        BaseFloat GetBaseFloat(string name);
        BaseInt GetBaseInt(string name);
        BaseInt AddBaseInt(string name, int val);
        BaseFloat AddBaseFloat(string name, float val);
        BaseFloat SetBaseFloat(string name, float val);
        BaseInt SetBaseInt(string name, int val);
        float GetFloat(string name);
        int GetInt(string name);
        void SetFloat(string name, float val);
        void SetInt(string name, int val);
        void IncreaseFloat(string name, float val);
        void IncreaseInt(string name, int val);

        public float this[string key] { get; set; }
    }

    
}