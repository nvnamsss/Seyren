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

    public static class AttributeName
    {
        public const string STRENGTH = "Strength";
        public const string INTELLIGENT = "Intelligent";
        public const string AGILITY = "Agility";
        public const string ATTACK_DAMAGE = "AttackDamage";
        public const string M_DAMAGE_AMPLIFIED = "MDamageAmplified";
        public const string MAX_HP = "MaxHp";
        public const string MAX_MP = "MaxMp";
        public const string CUR_HP = "CurHp";
        public const string CUR_MP = "CurMp";
        public const string HP_REGEN = "HpRegen";
        public const string MP_REGEN = "MpRegen";
        public const string SHIELD_REGEN = "ShieldRegen";
        public const string M_SHIELD_REGEN = "MShieldRegen";
        public const string P_SHIELD = "PShield";
        public const string M_SHIELD = "MShield";
        public const string HP_REGEN_PERCENT = "HpRegenPercent";
        public const string MP_REGEN_PERCENT = "MpRegenPercent";
        public const string DEFENSE = "Defense";
        public const string M_ARMOR = "MArmor";
        public const string ATTACK_RANGE = "AttackRange";
        public const string CAST_RANGE = "CastRange";
        public const string MOVEMENT_SPEED = "MovementSpeed";
        public const string ATTACK_SPEED = "AttackSpeed";
        public const string TURN_RATE = "TurnRate";
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