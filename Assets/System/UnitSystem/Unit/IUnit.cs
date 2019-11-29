using Crom.System.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crom.System.UnitSystem
{
    public interface IUnit : IObject, IAttribute
    {
        int CustomValue { get; set; }
        bool Targetable { get; set; }
        bool Invulnerable { get; set; }
        IUnit Owner { get; set; }
        IAttachable Attach { get; set; }
        float TimeScale { get; set; }
        float CurrentHp { get; set; }
        float CurrentMp { get; set; }
        float CurrentShield { get; set; }
        float CurrentMShield { get; set; }
        float CurrentPShield { get; set; }
        void Damage(IUnit source, DamageType type);
        void Damage(IUnit source, float damage, DamageType type, TriggerType trigger);
        void Damage(IUnit source, DamageType type, TriggerType trigger);
    }
}

