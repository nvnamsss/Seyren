using Crom.System.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crom.System.UnitSystem
{
    public interface IUnit
    {
        int CustomValue { get; set; }
        bool Targetable { get; set; }
        bool Invulnerable { get; set; }
        Unit Owner { get; set; }
        void Damage(Unit target, DamageType type);
        void Damage(Unit target, float damage, DamageType type);
    }
}

