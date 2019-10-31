using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Crom.System.UnitSystem
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
        public float CurrentHp { get; set; }
        public float CurrentMp { get; set; }
        public float HpRegen { get; set; }
        public float MpRegen { get; set; }
        public float HpRegenPercent { get; set; }
        public float MpRegenPercent { get; set; }
        public float Armor { get; set; }
        public float MArmor { get; set; }
        public float Shield { get; set; }
        public float MagicShield { get; set; }
        public float PhysicalShield { get; set; }

        void Start()
        {
            //UnityEngine.Debug.Log("[Attribute] - " + "Mana regen: " + MpRegen);
        }

        public void Update()
        {
            CurrentHp += HpRegen;
            //Log("Current hp: " + CurrentHp);
        }

        private void Log(object message)
        {
            Debug.Log("[Attribute] - " + message);
        }
    }
}
