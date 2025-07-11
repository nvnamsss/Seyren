using System;

namespace Seyren.System.Damages.Modifier
{
    [Serializable]
    public class CriticalModifier : IModifier
    {
        public float CriticalChance = 0.1f;
        public float CriticalMultiplier = 2f;
        public CriticalModifier(float criticalChance, float criticalMultiplier)
        {
            CriticalChance = criticalChance;
            CriticalMultiplier = criticalMultiplier;
        }

        public CriticalModifier()
        {
        }

        public void Apply(Damage damage)
        {
            if (UnityEngine.Random.value < CriticalChance)
            {
                damage.CriticalDamage += damage.BaseDamage * CriticalMultiplier;
            }
        }
    }
}