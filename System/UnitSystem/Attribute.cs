using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
namespace Base2D.System.UnitSystem
{
    [Serializable]
    public class Attribute 
    {
        public int DataType { get; set; }
        public float Strength
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
        public float Agility
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
        public float Intelligent
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
        public float AttackDamage
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
        public float MDamageAmplified
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
        public float MaxHp
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
        public float MaxMp
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
        public float HpRegen
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
        public float MpRegen
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
        public float Armor
        {
            get
            {
                return _armor;
            }
            set
            {
                _armor = value;
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
        public float AttackRange
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
        public float CastRange
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
        public float MovementSpeed
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
        public float AttackSpeed
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
        public float JumpSpeed
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
        private float _strength;
        [SerializeField]
        private float _agility;
        [SerializeField]
        private float _intelligent;
        [Header("Attack Settings")]
        [SerializeField]
        private float _attackDamage;
        [SerializeField]
        private float _mDamageAmplified;
        [SerializeField]
        [Header("State Settings")]
        private float _maxHp;
        [SerializeField]
        private float _maxMp;
        [SerializeField]
        private float _hpRegen;
        [SerializeField]
        private float _mpRegen;
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
        private float _armor;
        [SerializeField]
        private float _mArmor;
        [Header("Speed Settings")]
        [SerializeField]
        private float _attackRange;
        [SerializeField]
        private float _castRange;
        [SerializeField]
        private float _movementSpeed;
        [SerializeField]
        private float _attackSpeed;
        [SerializeField]
        private float _jumpSpeed;

        public static Attribute zero => new Attribute();
        public static Attribute operator +(Attribute lhs, Attribute rhs)
        {
            lhs.Strength += rhs.Strength;
            lhs.Agility += rhs.Agility;
            lhs.Intelligent += rhs.Intelligent;

            lhs.AttackDamage += rhs.AttackDamage;
            lhs.MDamageAmplified += rhs.MDamageAmplified;

            lhs.MaxHp += rhs.MaxHp;
            lhs.MaxMp += rhs.MaxMp;
            lhs.HpRegen += rhs.HpRegen;
            lhs.MpRegen += rhs.MpRegen;
            lhs.ShieldRegen += rhs.ShieldRegen;
            lhs.MShieldRegen += rhs.MShieldRegen;
            lhs.PShield += rhs.PShield;
            lhs.MpRegenPercent += rhs.MpRegenPercent;
            lhs.HpRegenPercent += rhs.HpRegenPercent;
            lhs.MpRegenPercent += rhs.MpRegenPercent;

            lhs.Armor += rhs.Armor;
            lhs.MArmor += rhs.MArmor;

            lhs.AttackRange += rhs.AttackRange;
            lhs.CastRange += rhs.CastRange;

            lhs.MovementSpeed += rhs.MovementSpeed;
            lhs.AttackSpeed += rhs.AttackSpeed;
            lhs.JumpSpeed += rhs.JumpSpeed;
            return lhs;
        }

        public static Attribute operator -(Attribute lhs, Attribute rhs)
        {
            lhs.Strength -= rhs.Strength;
            lhs.Agility -= rhs.Agility;
            lhs.Intelligent -= rhs.Intelligent;

            lhs.AttackDamage -= rhs.AttackDamage;
            lhs.MDamageAmplified -= rhs.MDamageAmplified;

            lhs.MaxHp -= rhs.MaxHp;
            lhs.MaxMp -= rhs.MaxMp;
            lhs.HpRegen -= rhs.HpRegen;
            lhs.MpRegen -= rhs.MpRegen;
            lhs.ShieldRegen -= rhs.ShieldRegen;
            lhs.MShieldRegen -= rhs.MShieldRegen;
            lhs.PShield -= rhs.PShield;
            lhs.MpRegenPercent -= rhs.MpRegenPercent;
            lhs.HpRegenPercent -= rhs.HpRegenPercent;
            lhs.MpRegenPercent -= rhs.MpRegenPercent;

            lhs.Armor -= rhs.Armor;
            lhs.MArmor -= rhs.MArmor;

            lhs.AttackRange -= rhs.AttackRange;
            lhs.CastRange -= rhs.CastRange;

            lhs.MovementSpeed -= rhs.MovementSpeed;
            lhs.AttackSpeed -= rhs.AttackSpeed;
            lhs.JumpSpeed -= rhs.JumpSpeed;
            return lhs;
        }

    }
}
