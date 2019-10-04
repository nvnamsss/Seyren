using System.Collections.Generic;
using UnityEngine;

namespace Crom.System.DamageSystem.Critical
{
    public class CriticalInfo : IDamageModification, IModifier
    {
        /**
         * 1: Physical damage only
         * 2: Magical damage only
         * 3: Both
         **/
        public static readonly CriticalInfo None = new CriticalInfo();
        public string Id { get; set; }
        public string Name { get; set; }
        public float Chance { get; set; }
        public float Multiple { get; set; }
        public float Bonus { get; set; }
        public List<IDamageModification> Stacks { get; set; }
        public StackType StackType { get; set; }
        public bool CanEvade { get; set; }
        public bool CanIgnoreArmor { get; set; }
        public bool CanIgnoreBlock { get; set; }

        public int CriticalType;
        

        public void Critical(DamageInfo damageInfo)
        {
            float chance = Random.Range(0, 100);
            Debug.Log(Name + ":" + chance);
            if (chance <= Chance)
            {
                Debug.Log(Name + ": Critical!!!");
                float damage = damageInfo.rawDamage * (Multiple - 1.0f);
                Debug.Log("Bonus:" + damage);
                damageInfo.DamageAmount += damage;
                damageInfo.increasedDamage += damage;
                damageInfo.CanEvade = CanEvade;
                damageInfo.CanIgnoreArmor = CanIgnoreArmor;
                damageInfo.CanIgnoreBlock = CanIgnoreBlock;
                Debug.Log("Damage: " + damageInfo.DamageAmount);
            }
        }
    }
}
