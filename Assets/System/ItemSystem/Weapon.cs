using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crom.System.DamageSystem;

namespace Assets.System.ItemSystem
{
    class Weapon : Item
    {
        public string type { get; } = "Weapon";

        public int ammount { get; set; }


        public string name { get;set; }
        public string description { get;set; }
        public int DataType { get;set; }
        public float Strength { get;set; }
        public float Agility { get;set; }
        public float Intelligent { get;set; }
        public float AttackDamage { get;set; }
        public float MDamageAmplified { get;set; }
        public float MaxHp { get;set; }
        public float MaxMp { get;set; }
        public float CurrentHp { get;set; }
        public float CurrentMp { get;set; }
        public float HpRegen { get;set; }
        public float MpRegen { get;set; }
        public float HpRegenPercent { get;set; }
        public float MpRegenPercent { get;set; }
        public float Armor { get;set; }
        public float MArmor { get;set; }
        public float Shield { get;set; }
        public float MagicShield { get;set; }
        public float PhysicalShield { get;set; }
        public ModificationInfos Modification { get;set; }
        public float LifeSteal { get;set; }
        public float MagicLifeSteal { get;set; }
        public float AttackSpeed { get;set; }
        public float CDReduction { get;set; }
        public float AttackRange { get;set; }
        public float CastRange { get;set; }
    }
}
