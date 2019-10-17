using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.System.BuffSystem
{
    [Flags()]    
    public enum BuffType
    {
        Size,
        Height,
        AnimationSpeed,
        TurnSpeed,
        Strength,
        Agility,
        Intelligent,
        AttackDamage,
        MDamageAmplified,
        MaxHp,
        MaxMp,
        CurrentHp,
        CurrentMp,
        HpRegen,
        MpRegen,
        HpRegenPercent,
        MpRegenPercent,
        Armor,
        MArmor,
        Shield,
        MagicShield,
        PhysicalShield,
        LifeSteal,
        MagicLifeSteal,
        AttackSpeed,
        CDReduction,
        AttackRange,
        CastRange
    }
}