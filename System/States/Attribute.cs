using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.System.States
{
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

    public struct BaseFloat
    {
        public static BaseFloat Zero = new BaseFloat(0, 0);
        public float Base;
        public float Incr;
        public float Total => Base + Incr;
        public BaseFloat(float b, float i)
        {
            Base = b;
            Incr = i;
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

    public interface IAttribute
    {
        BaseFloat GetBaseFloat(string name);
        BaseInt GetBaseInt(string name);
        BaseInt AddBaseInt(string name, int val);
        BaseFloat AddBaseFloat(string name, float val);
        BaseFloat SetBaseFloat(string name, float val);
        BaseInt SetBaseInt(string name, int val);
    }

    public class DefinelessAttribute : IAttribute
    {

        Dictionary<string, BaseFloat> baseFloats;
        Dictionary<string, BaseInt> baseInts;
        Dictionary<string, int> ints;
        Dictionary<string, float> floats;

        public DefinelessAttribute()
        {
            baseFloats = new Dictionary<string, BaseFloat>();
            baseInts = new Dictionary<string, BaseInt>();
            ints = new Dictionary<string, int>();
            floats = new Dictionary<string, float>();
        }

        public BaseFloat AddBaseFloat(string name, float val)
        {
            if (!baseFloats.ContainsKey(name))
            {
                baseFloats.Add(name, new BaseFloat(0, val));
                return baseFloats[name];
            }

            BaseFloat bf = baseFloats[name];
            bf.Increase(val);
            baseFloats[name] = bf;

            return bf;
        }

        public BaseInt AddBaseInt(string name, int val)
        {
            if (!baseInts.ContainsKey(name))
            {
                baseInts.Add(name, new BaseInt(0, val));
                return baseInts[name];
            }

            BaseInt bi = baseInts[name];
            bi.Increase(val);
            baseInts[name] = bi;

            return bi;
        }

        public BaseFloat GetBaseFloat(string name)
        {
            if (!baseFloats.ContainsKey(name))
            {
                return BaseFloat.Zero;
            }
            return baseFloats[name];
        }

        public BaseInt GetBaseInt(string name)
        {
            if (!baseInts.ContainsKey(name))
            {
                return BaseInt.Zero;
            }
            return baseInts[name];
        }

        public BaseFloat SetBaseFloat(string name, float val)
        {
            if (!baseFloats.ContainsKey(name))
            {
                baseFloats.Add(name, new BaseFloat(val, 0));
                return baseFloats[name];
            }

            BaseFloat bf = baseFloats[name];
            bf.Base = val;
            baseFloats[name] = bf;
            return bf;

        }

        public BaseInt SetBaseInt(string name, int val)
        {
            if (!baseInts.ContainsKey(name))
            {
                baseInts.Add(name, new BaseInt(val, 0));
                return baseInts[name];
            }
            BaseInt bi = baseInts[name];
            bi.Base = val;
            baseInts[name] = bi;
            return bi;
        }
    }

    [Serializable]
    public class Attribute : IAttribute
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

        public BaseFloat GetBaseFloat(string name)
        {
            switch (name)
            {
                case "Strength":
                    return Strength;
                case "Intelligent":
                    return Intelligent;
                case "Agility":
                    return Agility;
                case "HpRegen":
                    return HpRegen;
                case "MpRegen":
                    return MpRegen;
                case "CastRange":
                    return CastRange;
                case "AttackRange":
                    return AttackRange;
                case "AttackDamage":
                    return AttackDamage;
                case "MaxHp":
                    return MaxHp;
                case "MaxMp":
                    return MaxMp;
                case "MovementSpeed":
                    return MovementSpeed;
                case "MDamageAmplified":
                    return MDamageAmplified;
            }

            return BaseFloat.Zero;
        }

        public BaseInt GetBaseInt(string name)
        {
            return BaseInt.Zero;
        }

        public BaseInt AddBaseInt(string name, int val)
        {
            return BaseInt.Zero;
        }

        public BaseFloat AddBaseFloat(string name, float val)
        {
            switch (name)
            {
                case "Strength":
                    _strength.Increase(val);
                    return Strength;
                case "Intelligent":
                    _intelligent.Increase(val);
                    return Intelligent;
                case "Agility":
                    _agility.Increase(val);
                    return Agility;
                case "HpRegen":
                    _hpRegen.Increase(val);
                    return HpRegen;
                case "MpRegen":
                    _mpRegen.Increase(val);
                    return MpRegen;
                case "CastRange":
                    _castRange.Increase(val);
                    return CastRange;
                case "AttackRange":
                    _attackDamage.Increase(val);
                    return AttackRange;
                case "AttackDamage":
                    _attackDamage.Increase(val);
                    return AttackDamage;
                case "MaxHp":
                    _maxHp.Increase(val);
                    return MaxHp;
                case "MaxMp":
                    _agility.Increase(val);
                    return MaxMp;
                case "MovementSpeed":
                    _movementSpeed.Increase(val);
                    return MovementSpeed;
                case "MDamageAmplified":
                    _mDamageAmplified.Increase(val);
                    return MDamageAmplified;
            }

            return BaseFloat.Zero;
        }

        public BaseFloat SetBaseFloat(string name, float val)
        {
            switch (name)
            {
                case "Strength":
                    _strength.Base = val;
                    return Strength;
                case "Intelligent":
                    _intelligent.Base = val;
                    return Intelligent;
                case "Agility":
                    _agility.Base = val;
                    return Agility;
                case "HpRegen":
                    _hpRegen.Base = val;

                    return HpRegen;
                case "MpRegen":
                    _mpRegen.Base = val;
                    return MpRegen;
                case "CastRange":
                    _castRange.Base = val;
                    return CastRange;
                case "AttackRange":
                    _attackDamage.Base = val;
                    return AttackRange;
                case "AttackDamage":
                    _attackDamage.Base = val;
                    return AttackDamage;
                case "MaxHp":
                    _maxHp.Base = val;
                    return MaxHp;
                case "MaxMp":
                    _agility.Base = val;
                    return MaxMp;
                case "MovementSpeed":
                    _movementSpeed.Base = val;
                    return MovementSpeed;
                case "MDamageAmplified":
                    _mDamageAmplified.Base = val;
                    return MDamageAmplified;
            }

            return BaseFloat.Zero;
        }

        public BaseInt SetBaseInt(string name, int val)
        {
            return BaseInt.Zero;
        }
    }
}