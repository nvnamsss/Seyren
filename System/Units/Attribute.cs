using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;

namespace Seyren.System.Units
{
    public class BaseInt
    {
        public int Base;
        public int Incr;
        public int Total => Base + Incr;
        public BaseInt(int b, int i) {
            Base = b;
            Incr = i;
        }

        public int Amplify(float percent) {

            return Total;
        }

        public int Increase(int value) {
            return Total;
        }
    }

    public struct BaseFloat {
        public float Base;
        public float Incr;
        public float Total => Base + Incr;
        public BaseFloat(int b, int i) {
            Base = b;
            Incr = i;
        }

        public float Amplify(float percent) {
            Incr = Base * percent / 100;
            return Total;
        }

        public float Increase(int value) {
            Incr += value;
            return Total;
        }

        public static BaseFloat operator +(BaseFloat lhs, BaseFloat rhs) {
            lhs.Base += rhs.Base;
            lhs.Incr += rhs.Incr;
            return lhs;           
        }
        public static BaseFloat operator -(BaseFloat lhs, BaseFloat rhs) {
            lhs.Base -= rhs.Base;
            lhs.Incr -= rhs.Incr;
            return lhs;
        }

        public static BaseFloat operator +(BaseFloat lhs, float rhs) {
            lhs.Base += rhs;
            return lhs;
        }

        public static BaseFloat operator -(BaseFloat lhs, float rhs) {
            return lhs + (-rhs);
        }


    }
    
    [Serializable]
    public class Attribute 
    {
        public int DataType { get; set; }
        public BaseFloat Strength
        {
            get
            {
                return _strength;
            }
            set
            {
                _strength = value;
            }
        }
        public BaseFloat Agility
        {
            get
            {
                return _agility;
            }
            set
            {
                _agility = value;
            }
        }
        public BaseFloat Intelligent
        {
            get
            {
                return _intelligent;
            }
            set
            {
                _intelligent = value;
            }
        }
        public BaseFloat AttackDamage
        {
            get
            {
                return _attackDamage;
            }
            set
            {
                _attackDamage = value;
            }
        }
        public BaseFloat MDamageAmplified
        {
            get
            {
                return _mDamageAmplified;
            }
            set
            {
                _mDamageAmplified = value;
            }
        }
        public BaseFloat MaxHp
        {
            get
            {
                return _maxHp;
            }
            set
            {
                _maxHp = value; 
            }
        }
        public BaseFloat MaxMp
        {
            get
            {
                return _maxMp;
            }
            set
            {
                _maxMp = value;
            }
        }
        public BaseFloat HpRegen
        {
            get
            {
                return _hpRegen;
            }
            set
            {
                _hpRegen = value;
            }
        }
        public BaseFloat MpRegen
        {
            get
            {
                return _mpRegen;
            }
            set
            {
                _mpRegen = value;
            }
        }
        public float ShieldRegen
        {
            get
            {
                return _shieldRegen;
            }
            set
            {
                _shieldRegen = value;
            }
        }
        public float MShieldRegen
        {
            get
            {
                return _mShieldRegen;
            }
            set
            {
                _mShieldRegen = value;
            }
        }
        public float PShield
        {
            get
            {
                return _pShield;
            }
            set
            {
                _pShield = value;
            }
        }
        public float MShield
        {
            get
            {
                return _mShield;
            }
            set
            {
                _mShield = value;
            }
        }
        public float HpRegenPercent
        {
            get
            {
                return _hpRegenPercent;
            }
            set
            {
                _hpRegenPercent = value;
            }
        }
        public float MpRegenPercent
        {
            get
            {
                return _mpRegenPercent;
            }
            set
            {
                _mpRegenPercent = value;
            }
        }
        public BaseFloat Defense
        {
            get
            {
                return _defense;
            }
            set
            {
                _defense = value;
            }
        }
        public float MArmor
        {
            get
            {
                return _mArmor;
            }
            set
            {
                _mArmor = value;
            }
        }
        public BaseFloat AttackRange
        {
            get
            {
                return _attackRange;
            }
            set
            {
                _attackRange = value;
            }
        }
        public BaseFloat CastRange
        {
            get
            {
                return _castRange;
            }
            set
            {
                _castRange = value;
            }
        }
        public BaseFloat MovementSpeed
        {
            get
            {
                return _movementSpeed;
            }
            set
            {
                _movementSpeed = value;
            }
        }
        public BaseFloat AttackSpeed
        {
            get
            {
                return _attackSpeed;
            }
            set
            {
                _attackSpeed = value;
            }
        }
        public BaseFloat JumpSpeed
        {
            get
            {
                return _jumpSpeed;
            }
            set
            {
                _jumpSpeed = value;
            }
        }

        [SerializeField]
        private BaseFloat _strength;
        [SerializeField]
        private BaseFloat _agility;
        [SerializeField]
        private BaseFloat _intelligent;
        [Header("Attack Settings")]
        [SerializeField]
        private BaseFloat _attackDamage;
        [SerializeField]
        private BaseFloat _mDamageAmplified;
        [SerializeField]
        [Header("State Settings")]
        private BaseFloat _maxHp;
        [SerializeField]
        private BaseFloat _maxMp;
        [SerializeField]
        private BaseFloat _hpRegen;
        [SerializeField]
        private BaseFloat _mpRegen;
        [SerializeField]
        private float _shieldRegen;
        [SerializeField]
        private float _mShieldRegen;
        [SerializeField]
        private float _pShield;
        [SerializeField]
        private float _mShield;
        [SerializeField]
        private float _hpRegenPercent;
        [SerializeField]
        private float _mpRegenPercent;
        [SerializeField]
        private BaseFloat _defense;
        [SerializeField]
        private float _mArmor;
        [Header("Speed Settings")]
        [SerializeField]
        private BaseFloat _attackRange;
        [SerializeField]
        private BaseFloat _castRange;
        [SerializeField]
        private BaseFloat _movementSpeed;
        [SerializeField]
        private BaseFloat _attackSpeed;
        [SerializeField]
        private BaseFloat _jumpSpeed;

        public static Attribute zero => new Attribute();
        public static Attribute operator +(Attribute lhs, Attribute rhs)
        {
            // lhs.Strength += rhs.Strength;
            // lhs.Agility += rhs.Agility;
            // lhs.Intelligent += rhs.Intelligent;

            // lhs.AttackDamage += rhs.AttackDamage;
            // lhs.MDamageAmplified += rhs.MDamageAmplified;

            // lhs.MaxHp += rhs.MaxHp;
            // lhs.MaxMp += rhs.MaxMp;
            // lhs.HpRegen += rhs.HpRegen;
            // lhs.MpRegen += rhs.MpRegen;
            // lhs.ShieldRegen += rhs.ShieldRegen;
            // lhs.MShieldRegen += rhs.MShieldRegen;
            // lhs.PShield += rhs.PShield;
            // lhs.MpRegenPercent += rhs.MpRegenPercent;
            // lhs.HpRegenPercent += rhs.HpRegenPercent;
            // lhs.MpRegenPercent += rhs.MpRegenPercent;

            // lhs.Defense += rhs.Defense;
            // lhs.MArmor += rhs.MArmor;

            // lhs.AttackRange += rhs.AttackRange;
            // lhs.CastRange += rhs.CastRange;

            // lhs.MovementSpeed += rhs.MovementSpeed;
            // lhs.AttackSpeed += rhs.AttackSpeed;
            // lhs.JumpSpeed += rhs.JumpSpeed;
            return lhs;
        }

        public static Attribute operator -(Attribute lhs, Attribute rhs)
        {
            // lhs.Strength -= rhs.Strength;
            // lhs.Agility -= rhs.Agility;
            // lhs.Intelligent -= rhs.Intelligent;

            // lhs.AttackDamage -= rhs.AttackDamage;
            // lhs.MDamageAmplified -= rhs.MDamageAmplified;

            // lhs.MaxHp -= rhs.MaxHp;
            // lhs.MaxMp -= rhs.MaxMp;
            // lhs.HpRegen -= rhs.HpRegen;
            // lhs.MpRegen -= rhs.MpRegen;
            // lhs.ShieldRegen -= rhs.ShieldRegen;
            // lhs.MShieldRegen -= rhs.MShieldRegen;
            // lhs.PShield -= rhs.PShield;
            // lhs.MpRegenPercent -= rhs.MpRegenPercent;
            // lhs.HpRegenPercent -= rhs.HpRegenPercent;
            // lhs.MpRegenPercent -= rhs.MpRegenPercent;

            // lhs.Defense -= rhs.Defense;
            // lhs.MArmor -= rhs.MArmor;

            // lhs.AttackRange -= rhs.AttackRange;
            // lhs.CastRange -= rhs.CastRange;

            // lhs.MovementSpeed -= rhs.MovementSpeed;
            // lhs.AttackSpeed -= rhs.AttackSpeed;
            // lhs.JumpSpeed -= rhs.JumpSpeed;
            return lhs;
        }

    }
}
