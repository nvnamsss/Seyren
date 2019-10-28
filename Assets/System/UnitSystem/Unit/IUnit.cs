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
        void Damage(IUnit target, DamageType type);
        void Damage(IUnit target, float damage, DamageType type);
    }
}

