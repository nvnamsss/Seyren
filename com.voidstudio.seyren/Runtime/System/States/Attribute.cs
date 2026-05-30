using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.System.States
{
    [Serializable]
    public class BaseInt
    {
        public static BaseInt Zero = new BaseInt(0, 0);

        public int Base;
        public int Incr;
        public int Floor { get; set; } = int.MinValue;
        public int Ceiling { get; set; } = int.MaxValue;
        public int Total => UnityEngine.Mathf.Clamp(Base + Incr, Floor, Ceiling);
        
        public BaseInt(int b, int i)
        {
            Base = b;
            Incr = i;
        }

        public BaseInt(int b, int i, int floor, int ceiling)
        {
            Base = b;
            Incr = i;
            Floor = floor;
            Ceiling = ceiling;
        }

        public int Amplify(float percent)
        {
            return Total;
        }

        public int Increase(int value)
        {
            Incr += value;
            return Total;
        }

        /// <summary>
        /// Sets the floor (minimum) value for this attribute
        /// </summary>
        /// <param name="minValue">Minimum allowed value</param>
        /// <returns>Current total value after applying constraints</returns>
        public int SetFloor(int minValue)
        {
            Floor = minValue;
            return Total;
        }

        /// <summary>
        /// Sets the ceiling (maximum) value for this attribute
        /// </summary>
        /// <param name="maxValue">Maximum allowed value</param>
        /// <returns>Current total value after applying constraints</returns>
        public int SetCeiling(int maxValue)
        {
            Ceiling = maxValue;
            return Total;
        }

        /// <summary>
        /// Sets both floor and ceiling values
        /// </summary>
        /// <param name="minValue">Minimum allowed value</param>
        /// <param name="maxValue">Maximum allowed value</param>
        /// <returns>Current total value after applying constraints</returns>
        public int SetRange(int minValue, int maxValue)
        {
            Floor = minValue;
            Ceiling = maxValue;
            return Total;
        }
    }

    [Serializable]
    public class BaseFloat
    {
        public static BaseFloat Zero = new BaseFloat(0, 0);
        public float Base;
        public float Incr => incr;
        public float Floor { get; set; } = float.MinValue;
        public float Ceiling { get; set; } = float.MaxValue;
        public float Total => UnityEngine.Mathf.Clamp(Base + Incr, Floor, Ceiling);
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

        public BaseFloat(float baseValue, float increasedValue, float floor, float ceiling)
        {
            Base = baseValue;
            incr = increasedValue;
            Floor = floor;
            Ceiling = ceiling;
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

        /// <summary>
        /// Sets the floor (minimum) value for this attribute
        /// </summary>
        /// <param name="minValue">Minimum allowed value</param>
        /// <returns>Current total value after applying constraints</returns>
        public float SetFloor(float minValue)
        {
            Floor = minValue;
            return Total;
        }

        /// <summary>
        /// Sets the ceiling (maximum) value for this attribute
        /// </summary>
        /// <param name="maxValue">Maximum allowed value</param>
        /// <returns>Current total value after applying constraints</returns>
        public float SetCeiling(float maxValue)
        {
            Ceiling = maxValue;
            return Total;
        }

        /// <summary>
        /// Sets both floor and ceiling values
        /// </summary>
        /// <param name="minValue">Minimum allowed value</param>
        /// <param name="maxValue">Maximum allowed value</param>
        /// <returns>Current total value after applying constraints</returns>
        public float SetRange(float minValue, float maxValue)
        {
            Floor = minValue;
            Ceiling = maxValue;
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

    /// <summary>
    /// Warcraft 3-style attribute implementation with primary attributes affecting secondary stats
    /// </summary>
    [Serializable]
    public class Warcraft3Attribute : IAttribute
    {
        #region Primary Attributes
        [SerializeField] private BaseFloat _strength = new BaseFloat(10);
        [SerializeField] private BaseFloat _agility = new BaseFloat(10);
        [SerializeField] private BaseFloat _intelligence = new BaseFloat(10);
        #endregion

        #region Combat Attributes
        [SerializeField] private BaseFloat _attackDamage = new BaseFloat(0);
        [SerializeField] private BaseFloat _armor = new BaseFloat(0);
        [SerializeField] private BaseFloat _attackSpeed = new BaseFloat(1.0f);
        [SerializeField] private BaseFloat _magicResistance = new BaseFloat(0);
        #endregion

        #region Health & Mana
        [SerializeField] private BaseFloat _maxHp = new BaseFloat(100);
        [SerializeField] private BaseFloat _maxMp = new BaseFloat(100);
        [SerializeField] private BaseFloat _hpRegen = new BaseFloat(0.25f);
        [SerializeField] private BaseFloat _mpRegen = new BaseFloat(0.01f);
        #endregion

        #region Movement & Utility
        [SerializeField] private BaseFloat _movementSpeed = new BaseFloat(300);
        [SerializeField] private BaseFloat _attackRange = new BaseFloat(128);
        [SerializeField] private BaseFloat _castRange = new BaseFloat(600);
        [SerializeField] private BaseFloat _turnRate = new BaseFloat(0.6f);
        #endregion

        #region Current Values (not affected by bonuses)
        public float CurrentHp { get; set; }
        public float CurrentMp { get; set; }
        public int Level { get; set; } = 1;
        #endregion

        public Warcraft3Attribute(float str = 10, float agi = 10, float intel = 10)
        {
            _strength = new BaseFloat(str);
            _agility = new BaseFloat(agi);
            _intelligence = new BaseFloat(intel);

            // Initialize current values based on max values
            CurrentHp = GetMaxHp();
            CurrentMp = GetMaxMp();
        }

        #region Primary Attribute Properties with WC3 Bonuses
        public float Strength => _strength.Total;
        public float Agility => _agility.Total;
        public float Intelligence => _intelligence.Total;

        /// <summary>
        /// Total attack damage including strength bonus (1 damage per strength)
        /// </summary>
        public float AttackDamage => _attackDamage.Total + Strength;

        /// <summary>
        /// Total armor including agility bonus (1 armor per 7 agility)
        /// </summary>
        public float Armor => _armor.Total + (Agility / 7f);

        /// <summary>
        /// Maximum HP including strength bonus (19 HP per strength)
        /// </summary>
        public float GetMaxHp() => _maxHp.Total + (Strength * 19f);

        /// <summary>
        /// Maximum MP including intelligence bonus (13 MP per intelligence)
        /// </summary>
        public float GetMaxMp() => _maxMp.Total + (Intelligence * 13f);

        /// <summary>
        /// HP regeneration including strength bonus (0.03 HP/sec per strength)
        /// </summary>
        public float HpRegen => _hpRegen.Total + (Strength * 0.03f);

        /// <summary>
        /// MP regeneration including intelligence bonus (0.04 MP/sec per intelligence)
        /// </summary>
        public float MpRegen => _mpRegen.Total + (Intelligence * 0.04f);

        /// <summary>
        /// Attack speed including agility bonus (2% IAS per agility)
        /// </summary>
        public float AttackSpeed => _attackSpeed.Total + (Agility * 0.02f);

        /// <summary>
        /// Movement speed (can be modified by items/spells)
        /// </summary>
        public float MovementSpeed => _movementSpeed.Total;

        /// <summary>
        /// Magic resistance (can be modified by items/spells)
        /// </summary>
        public float MagicResistance => _magicResistance.Total;

        /// <summary>
        /// Attack range (can be modified by items/spells)
        /// </summary>
        public float AttackRange => _attackRange.Total;

        /// <summary>
        /// Cast range (can be modified by items/spells)
        /// </summary>
        public float CastRange => _castRange.Total;

        /// <summary>
        /// Turn rate (can be modified by items/spells)
        /// </summary>
        public float TurnRate => _turnRate.Total;
        #endregion

        #region IAttribute Implementation
        public BaseFloat GetBaseFloat(string name)
        {
            return name switch
            {
                AttributeName.STRENGTH => _strength,
                AttributeName.AGILITY => _agility,
                AttributeName.INTELLIGENT => _intelligence,
                AttributeName.ATTACK_DAMAGE => _attackDamage,
                AttributeName.DEFENSE => _armor,
                AttributeName.ATTACK_SPEED => _attackSpeed,
                AttributeName.M_ARMOR => _magicResistance,
                AttributeName.MAX_HP => _maxHp,
                AttributeName.MAX_MP => _maxMp,
                AttributeName.HP_REGEN => _hpRegen,
                AttributeName.MP_REGEN => _mpRegen,
                AttributeName.MOVEMENT_SPEED => _movementSpeed,
                AttributeName.ATTACK_RANGE => _attackRange,
                AttributeName.CAST_RANGE => _castRange,
                AttributeName.TURN_RATE => _turnRate,
                _ => BaseFloat.Zero
            };
        }

        public BaseInt GetBaseInt(string name)
        {
            return name switch
            {
                AttributeName.LEVEL => new BaseInt(Level, 0),
                _ => BaseInt.Zero
            };
        }

        public BaseFloat AddBaseFloat(string name, float val)
        {
            var baseFloat = GetBaseFloat(name);
            if (!baseFloat.Equals(BaseFloat.Zero))
            {
                baseFloat.Increase(val);
            }
            return baseFloat;
        }

        public BaseInt AddBaseInt(string name, int val)
        {
            if (name == AttributeName.LEVEL)
            {
                Level += val;
                return new BaseInt(Level, 0);
            }
            return BaseInt.Zero;
        }

        public BaseFloat SetBaseFloat(string name, float val)
        {
            var baseFloat = GetBaseFloat(name);
            if (!baseFloat.Equals(BaseFloat.Zero))
            {
                baseFloat.Base = val;
            }
            return baseFloat;
        }

        public BaseInt SetBaseInt(string name, int val)
        {
            if (name == AttributeName.LEVEL)
            {
                Level = val;
                return new BaseInt(Level, 0);
            }
            return BaseInt.Zero;
        }

        public float GetFloat(string name)
        {
            return name switch
            {
                AttributeName.CUR_HP => CurrentHp,
                AttributeName.CUR_MP => CurrentMp,
                AttributeName.STRENGTH => Strength,
                AttributeName.AGILITY => Agility,
                AttributeName.INTELLIGENT => Intelligence,
                AttributeName.ATTACK_DAMAGE => AttackDamage,
                AttributeName.DEFENSE => Armor,
                AttributeName.ATTACK_SPEED => AttackSpeed,
                AttributeName.M_ARMOR => MagicResistance,
                AttributeName.MAX_HP => GetMaxHp(),
                AttributeName.MAX_MP => GetMaxMp(),
                AttributeName.HP_REGEN => HpRegen,
                AttributeName.MP_REGEN => MpRegen,
                AttributeName.MOVEMENT_SPEED => MovementSpeed,
                AttributeName.ATTACK_RANGE => AttackRange,
                AttributeName.CAST_RANGE => CastRange,
                AttributeName.TURN_RATE => TurnRate,
                _ => 0f
            };
        }

        public int GetInt(string name)
        {
            return name switch
            {
                AttributeName.LEVEL => Level,
                _ => (int)GetFloat(name)
            };
        }

        public void SetFloat(string name, float val)
        {
            switch (name)
            {
                case AttributeName.CUR_HP:
                    CurrentHp = UnityEngine.Mathf.Clamp(val, 0, GetMaxHp());
                    break;
                case AttributeName.CUR_MP:
                    CurrentMp = UnityEngine.Mathf.Clamp(val, 0, GetMaxMp());
                    break;
                default:
                    SetBaseFloat(name, val);
                    break;
            }
        }

        public void SetInt(string name, int val)
        {
            if (name == AttributeName.LEVEL)
            {
                Level = val;
            }
            else
            {
                SetFloat(name, val);
            }
        }

        public void IncreaseFloat(string name, float val)
        {
            switch (name)
            {
                case AttributeName.CUR_HP:
                    CurrentHp = UnityEngine.Mathf.Clamp(CurrentHp + val, 0, GetMaxHp());
                    break;
                case AttributeName.CUR_MP:
                    CurrentMp = UnityEngine.Mathf.Clamp(CurrentMp + val, 0, GetMaxMp());
                    break;
                default:
                    AddBaseFloat(name, val);
                    break;
            }
        }

        public void IncreaseInt(string name, int val)
        {
            if (name == AttributeName.LEVEL)
            {
                Level += val;
            }
            else
            {
                IncreaseFloat(name, val);
            }
        }

        public float this[string key]
        {
            get => GetFloat(key);
            set => SetFloat(key, value);
        }

        #endregion

        #region Warcraft 3 Specific Methods

        /// <summary>
        /// Levels up the hero, increasing primary attributes based on hero type
        /// </summary>
        /// <param name="strGain">Strength gain per level</param>
        /// <param name="agiGain">Agility gain per level</param>
        /// <param name="intGain">Intelligence gain per level</param>
        public void LevelUp(float strGain = 2f, float agiGain = 2f, float intGain = 2f)
        {
            Level++;
            _strength.Base += strGain;
            _agility.Base += agiGain;
            _intelligence.Base += intGain;

            // Heal to full when leveling up (WC3 behavior)
            CurrentHp = GetMaxHp();
            CurrentMp = GetMaxMp();
        }

        /// <summary>
        /// Calculates damage reduction based on armor (WC3 formula)
        /// </summary>
        /// <returns>Damage reduction percentage (0-1)</returns>
        public float GetDamageReduction()
        {
            float armor = Armor;
            if (armor >= 0)
            {
                return armor / (armor + 100f);
            }
            else
            {
                return armor / (100f - armor);
            }
        }

        /// <summary>
        /// Calculates magic damage reduction based on magic resistance
        /// </summary>
        /// <returns>Magic damage reduction percentage (0-1)</returns>
        public float GetMagicDamageReduction()
        {
            return UnityEngine.Mathf.Clamp01(MagicResistance / 100f);
        }

        /// <summary>
        /// Gets the attack cooldown in seconds based on attack speed
        /// </summary>
        /// <param name="baseAttackTime">Base attack time (default 1.6 seconds for most units)</param>
        /// <returns>Attack cooldown in seconds</returns>
        public float GetAttackCooldown(float baseAttackTime = 1.6f)
        {
            return baseAttackTime / (1f + AttackSpeed);
        }

        /// <summary>
        /// Checks if the unit is at full health
        /// </summary>
        public bool IsAtFullHealth() => CurrentHp >= GetMaxHp();

        /// <summary>
        /// Checks if the unit is at full mana
        /// </summary>
        public bool IsAtFullMana() => CurrentMp >= GetMaxMp();

        /// <summary>
        /// Gets health percentage (0-1)
        /// </summary>
        public float GetHealthPercentage() => CurrentHp / GetMaxHp();

        /// <summary>
        /// Gets mana percentage (0-1)
        /// </summary>
        public float GetManaPercentage() => CurrentMp / GetMaxMp();

        #endregion
    }

    
}