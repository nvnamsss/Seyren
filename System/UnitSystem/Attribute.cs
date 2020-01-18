using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
namespace Base2D.System.UnitSystem
{
    public class Attribute : ScriptableObject
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
            }
        }
        public float ShieldRegen
        {
            get
            {
            }
            set
            {
            }
        }
        public float MShieldRegen
        {
            get
            {
            }
            set
            {
            }
        }
        public float PShield
        {
            get
            {
            }
            set
            {
            }
        }
        public float MShield
        {
            get
            {
            }
            set
            {
            }
        }
        public float HpRegenPercent
        {
            get
            {
            }
            set
            {
            }
        }
        public float MpRegenPercent
        {
            get
            {
            }
            set
            {
            }
        }
        public float Armor
        {
            get
            {
            }
            set
            {
            }
        }
        public float MArmor
        {
            get
            {
            }
            set
            {
            }
        }
        public float AttackRange
        {
            get
            {
            }
            set
            {
            }
        }
        public float CastRange
        {
            get
            {
            }
            set
            {
            }
        }
        public float MovementSpeed
        {
            get
            {
            }
            set
            {
            }
        }
        public float AttackSpeed
        {
            get
            {
            }
            set
            {
            }
        }
        public float JumpSpeed
        {
            get
            {
            }
            set
            {
            }
        }

        private float _strength;
        private float _agility;
        private float _intelligent;
        private float _attackDamage;
        private float _mDamageAmplified;
        private float _maxHp;
        private float _maxMp;
        private float _hpRegen;
        private float _mpRegen;
        private float _shieldRegen;
        private float _mShieldRegen;
        private float _pShield;
        private float _mShield;
        private float _hpRegenPercent;
        private float _mpRegenPercent;
        private float _armor;
        private float _mArmor;
        private float _attackRange;
        private float _castRange;
        private float _movementSpeed;
        private float _attackSpeed;
        private float _jumpSpeed;
        void Start()
        {
        }

        public void Update()
        {
        }

        private void Log(object message)
        {
            Debug.Log("[Attribute] - " + message);
        }
    }
}
