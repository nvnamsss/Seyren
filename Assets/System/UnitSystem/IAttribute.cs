using Crom.System.DamageSystem;
using Crom.System.DamageSystem.Critical;
using Crom.System.DamageSystem.Evasion;
using Crom.System.DamageSystem.Reduce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.UnitSystem
{
    interface IAttribute
    {
        int DataType { get; set; }
        float Strength { get; set; }
        float Agility { get; set; }
        float Intelligent { get; set; }
        float AttackDamage { get; set; }
        float MDamageAmplified { get; set; }
        float MaxHp { get; set; }
        float MaxMp { get; set; }
        float CurrentHp { get; set; }
        float CurrentMp { get; set; }
        float HpRegen { get; set; }
        float MpRegen { get; set; }
        float HpRegenPercent { get; set; }
        float MpRegenPercent { get; set; }
        float Armor { get; set; }
        float MArmor { get; set; }
        float Shield { get; set; }
        float MagicShield { get; set; }
        float PhysicalShield { get; set; }
        //CriticalInfo array CriticalInfos[1]{ get; set; }
        //int CriticalCount { get; set; }
        ModificationInfos Modification { get; set; }
        float LifeSteal { get; set; }
        float MagicLifeSteal { get; set; }

        float AttackSpeed { get; set; }
        float CDReduction { get; set; }

        float AttackRange { get; set; }
        float CastRange { get; set; }
    }
}
