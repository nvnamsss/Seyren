using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Damages.Evasion
{
    
    public abstract class EvasionInfo : IDamageModification<EvasionInfo>, IModifier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Chance { get; set; }
        public List<EvasionInfo> Stacks { get; set; }
        public StackType StackType { get; set; }
        public bool CanEvade { get; set; }
        public bool CanCritical { get; set; }
        public bool CanReduce { get; set; }

        public virtual void Trigger(Damage damageInfo)
        {
            float chance = UnityEngine.Random.Range(0, 100);
            Evaded(damageInfo, chance < Chance, chance);
        }

        public abstract void Evaded(Damage info, bool success, float chance);
    }
}
