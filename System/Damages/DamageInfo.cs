using Seyren.System.Damages.Critical;
using Seyren.System.Damages.Evasion;
using Seyren.System.Damages.PostPassive;
using Seyren.System.Damages.PrePassive;
using Seyren.System.Damages.Reduce;
using Seyren.System.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Damages
{
    public class DamageInfo: IModifier
    {
        public Unit Source { get; set; }
        public Unit Target { get; set; }
        public TriggerType TriggerType { get; set; }
        public DamageType DamageType { get; set; }
        public ModificationStatus DamageStatus { get; set; }
        public PrePassiveInfos PrePassive { get; set; }
        public CriticalInfos Critical { get; set; }
        public EvasionInfos Evasion { get; set; }
        public ReductionInfos Reduction { get; set; }
        public PostPassiveInfos PostPassive { get; set; }
        public float DamageAmount { get; set; }
        public bool CanCritical { get; set; }
        public bool CanEvade { get; set; }
        public bool CanReduce { get; set; }

        public Dictionary<DamageType, float> damages;
        public float rawDamage;
        public float increasedDamage;
        public float criticalDamage;
        public float reducedDamage;
        public float blockedDamage;
        public bool evaded;

        public DamageInfo(Unit source, Unit target)
        {
            Source = source;
            Target  = target;
        }

        public float CalculateDamage()
        {
            float damage = 0;
            bool triggerPrePassive = (TriggerType | TriggerType.PrePassive) == TriggerType;
            bool triggerCritical = (TriggerType | TriggerType.Critical) == TriggerType;
            bool triggerEvasion = (TriggerType | TriggerType.Evasion) == TriggerType;
            bool triggerReduction = (TriggerType | TriggerType.Reduction) == TriggerType;
            bool triggerPostPassive = (TriggerType | TriggerType.PostPassive) == TriggerType;

            rawDamage = DamageAmount;
            increasedDamage = 0;
            reducedDamage = 0;

            if (triggerPrePassive)
            {
                foreach (DictionaryEntry entry in PrePassive)
                {
                    CriticalInfo modifier = entry.Value as CriticalInfo;
                    modifier.Trigger(this);
                }
            }
            

            if (triggerCritical && CanCritical)
            {
                foreach (DictionaryEntry entry in Critical)
                {
                    CriticalInfo modifier = entry.Value as CriticalInfo;
                    modifier.Trigger(this);
                }
            }
            
            if (triggerEvasion && CanEvade)
            {
                foreach (KeyValuePair<int, EvasionInfo> entry in Evasion)
                {
                    entry.Value.Trigger(this);
                }
            }
            
            if (triggerReduction && CanReduce)
            {
                //foreach (DictionaryEntry entry in Reduction.Modifications)
                //{
                //    ReductionInfo modifier = entry.Value as ReductionInfo;
                //    modifier.Reduce(this);
                //}
            }
            
            if (triggerPostPassive)
            {
                //foreach (DictionaryEntry entry in PostPassive.Modifications)
                //{
                //    CriticalInfo modifier = entry.Value as CriticalInfo;
                //    modifier.Critical(this);
                //}
            }

            return damage;
        }

        public override string ToString()
        {
            string s = "Source: " + (Source as Unit).name + Environment.NewLine +
                "Target: " + (Target as Unit).name + Environment.NewLine +
                "PrePassive: " + ((TriggerType | TriggerType.PrePassive) == TriggerType) + Environment.NewLine +
                "Critical: " + ((TriggerType | TriggerType.Critical) == TriggerType) + Environment.NewLine +
                "Evasion: " + ((TriggerType | TriggerType.Evasion) == TriggerType) + Environment.NewLine +
                "Reduction: " + ((TriggerType | TriggerType.Reduction) == TriggerType) + Environment.NewLine +
                "PostPassive: " + ((TriggerType | TriggerType.PrePassive) == TriggerType) + Environment.NewLine +
                "DamageType: " + DamageType + Environment.NewLine +
                "DamageAmount: " + DamageAmount + Environment.NewLine +
                "RawDamage: " + rawDamage + Environment.NewLine +
                "IncreasedDamage: " + increasedDamage + Environment.NewLine +
                "ReduceDamage: " + reducedDamage;

            return s;
        }
    }
}
