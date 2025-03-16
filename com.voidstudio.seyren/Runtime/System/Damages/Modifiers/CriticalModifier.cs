using System;
using UnityEngine;

namespace Seyren.System.Damages.Modifier
{
    [Serializable]
    public class CriticalModifier : IModifier
    {
        public float CriticalChance = 0.1f;
        public float CriticalMultiplier = 2f;
        public void Apply(Damage damage)
        {
            if (UnityEngine.Random.value < CriticalChance)
            {
                damage.CriticalDamage += damage.BaseDamage * CriticalMultiplier;
            }
        }
    }
}