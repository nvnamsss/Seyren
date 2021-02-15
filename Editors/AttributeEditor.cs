using System.Collections;
using System.Collections.Generic;
using Seyren.System.Units;
using UnityEngine;

namespace Seyren.Editor
{
    public class AttributeEditor : MonoBehaviour
    {
        private Unit unit;
        private Attribute attribute;
        public float Strength;
        public float Agility;
        public float Intelligent;
        public float AttackDamage;
        public float MDamageAmplified;
        public float MaxHp;
        public float MaxMp;
        [Header("State Settings")]
        public float HpRegen;
        public float MpRegen;
        public float ShieldRegen;
        public float MShieldRegen;
        public float PShield;
        public float HpRegenPercent;
        public float MpRegenPercent;
        public float Armor;
        public float MArmor;
        [Header("State Settings")]
        public float AttackRange;
        public float CastRange;
        [Header("Speed Settings")]
        public float MovementSpeed;
        public float AttackSpeed;
        public float JumpSpeed;

        void Start()
        {
            unit = GetComponent<Unit>();
            attribute.Strength = Strength;
            attribute.Agility = Agility;
            attribute.Intelligent = Intelligent;

            attribute.AttackDamage = AttackDamage;
            attribute.MDamageAmplified = MDamageAmplified;

            attribute.MaxHp = MaxHp;
            attribute.MaxMp = MaxMp;
            attribute.HpRegen = HpRegen;
            attribute.MpRegen = MpRegen;
            attribute.ShieldRegen = ShieldRegen;
            attribute.MShieldRegen = MShieldRegen;
            attribute.PShield = PShield;
            attribute.HpRegenPercent = HpRegenPercent;
            attribute.MpRegenPercent = MpRegenPercent;

            attribute.Armor = Armor;
            attribute.MArmor = MArmor;

            attribute.AttackRange = AttackRange;
            attribute.CastRange = CastRange;

            attribute.MovementSpeed = MovementSpeed;
            attribute.AttackSpeed = AttackSpeed;
            attribute.JumpSpeed = JumpSpeed;
        }
    }
}

