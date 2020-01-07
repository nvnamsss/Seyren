using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base2D.System.UnitSystem
{
    public class Attribute
    {
        public int DataType { get; set; }
        public float Strength { get; set; }
        public float Agility { get; set; }
        public float Intelligent { get; set; }
        public float AttackDamage { get; set; }
        public float MDamageAmplified { get; set; }
        public float MaxHp { get; set; }
        public float MaxMp { get; set; }
        public float HpRegen { get; set; }
        public float MpRegen { get; set; }
        public float ShieldRegen { get; set; }
        public float MShieldRegen { get; set; }
        public float PShield { get; set; }
        public float HpRegenPercent { get; set; }
        public float MpRegenPercent { get; set; }
        public float Armor { get; set; }
        public float MArmor { get; set; }
        public float AttackRange { get; set; }
        public float CastRange { get; set; }
        public float MovementSpeed { get; set; }
        public float AttackSpeed { get; set; }
        public float JumpSpeed { get; set; }

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
