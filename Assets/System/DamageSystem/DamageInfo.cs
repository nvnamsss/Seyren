using Crom.System.DamageSystem.Critical;
using Crom.System.DamageSystem.Evasion;
using Crom.System.DamageSystem.PostPassive;
using Crom.System.DamageSystem.PrePassive;
using Crom.System.DamageSystem.Reduce;
using Crom.System.UnitSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.DamageSystem
{
    public class DamageInfo: IModifier
    {
        public Unit Source { get; set; }
        public Unit Target { get; set; }
        public bool TriggerAttack { get; set; }
        public DamageType Type { get; set; }
        public PrePassiveInfos PrePassive { get; set; }
        public CriticalInfos Critical { get; set; }
        public EvasionInfos Evasion { get; set; }
        public ReductionInfos Reduction { get; set; }
        public PostPassiveInfos PostPassive { get; set; }
        public float DamageAmount { get; set; }
        public bool CanEvade { get; set; }
        public bool CanIgnoreArmor { get; set; }
        public bool CanIgnoreBlock { get; set; }

        public float rawDamage;
        public float increasedDamage;
        public float reducedDamage;

        public DamageInfo(Unit source, Unit target)
        {
            Source = source;
            Target  = target;
        }


        public float CalculateDamage()
        {
            float damage = 0;
            rawDamage = DamageAmount;
            increasedDamage = 0;
            reducedDamage = 0;

            foreach (DictionaryEntry entry in PrePassive.Modifications)
            {
                CriticalInfo modifier = entry.Value as CriticalInfo;
                modifier.Critical(this);
            }

            foreach (DictionaryEntry entry in Critical.Modifications)
            {
                CriticalInfo modifier = entry.Value as CriticalInfo;
                modifier.Critical(this);
            }

            foreach (DictionaryEntry entry in Evasion.Modifications)
            {
                EvasionInfo modifier = entry.Value as EvasionInfo;
                modifier.Evade(this);
            }

            foreach (DictionaryEntry entry in Reduction.Modifications)
            {
                ReductionInfo modifier = entry.Value as ReductionInfo;
                modifier.Reduce(this);
            }

            foreach (DictionaryEntry entry in PostPassive.Modifications)
            {
                CriticalInfo modifier = entry.Value as CriticalInfo;
                modifier.Critical(this);
            }

            return damage;
        }
    }
}
