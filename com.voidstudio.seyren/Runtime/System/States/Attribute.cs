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
    public struct BaseFloat
    {
        public static BaseFloat Zero = new BaseFloat(0, 0);
        public float Base;
        public float Incr;
        public float Total => Base + Incr;

        public BaseFloat(float baseValue) : this(baseValue, 0)
        {

        }

        public BaseFloat(float baseValue, float increasedValue)
        {
            Base = baseValue;
            Incr = increasedValue;
        }

        public float Amplify(float percent)
        {
            Incr = Base * percent / 100;
            return Total;
        }

        public float Increase(float value)
        {
            Incr += value;
            return Total;
        }

        public static BaseFloat operator +(BaseFloat lhs, BaseFloat rhs)
        {
            lhs.Base += rhs.Base;
            lhs.Incr += rhs.Incr;
            return lhs;
        }
        public static BaseFloat operator -(BaseFloat lhs, BaseFloat rhs)
        {
            lhs.Base -= rhs.Base;
            lhs.Incr -= rhs.Incr;
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
    }

    
}