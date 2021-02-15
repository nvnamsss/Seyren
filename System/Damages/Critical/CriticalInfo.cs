using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Seyren.System.Damages.Critical
{
    public abstract class CriticalInfo : IDamageModification<CriticalInfo>, IModifier
    {
        /**
         * 1: Physical damage only
         * 2: Magical damage only
         * 3: Both
         **/
        public int Id { get; set; }
        public string Name { get; set; }
        public float Chance { get; set; }
        public float Multiple { get; set; }
        public float Bonus { get; set; }
        public List<CriticalInfo> Stacks { get; set; }
        public StackType StackType { get; set; }
        public bool CanEvade { get; set; }
        public bool CanCritical { get; set; }
        public bool CanReduce { get; set; }

        public int CriticalType;
        [SerializeField]
        private string _name;
        [SerializeField]
        private float _chance;
        [SerializeField]
        private float _multiple;
        [SerializeField]
        private float _bonus;
        [SerializeField]
        private StackType stackType;
        [SerializeField]
        private bool _canEvade;
        [SerializeField]
        private bool _canCritical;
        [SerializeField]
        private bool _canReduce;
        public virtual void Trigger(DamageInfo damageInfo)
        {
            float chance = UnityEngine.Random.Range(0, 100);
            Debug.Log(Name + ":" + chance);
            if (chance <= Chance)
            {
                Debug.Log(Name + ": Critical!!!");
                float damage = damageInfo.rawDamage * (Multiple - 1.0f);
                Debug.Log("Bonus:" + damage);
                damageInfo.DamageAmount += damage;
                damageInfo.increasedDamage += damage;
                damageInfo.CanEvade = CanEvade;
                damageInfo.CanCritical = CanCritical;
                damageInfo.CanReduce = CanReduce;
                Debug.Log("Damage: " + damageInfo.DamageAmount);
            }
            Critical(damageInfo, chance <= Chance, chance);
        }

        public abstract void Critical(DamageInfo info, bool success, float chance);
    }
}
