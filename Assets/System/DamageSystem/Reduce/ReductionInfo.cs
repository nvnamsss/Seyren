using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.DamageSystem.Reduce
{
    public class ReductionInfo : IDamageModification, IModifier
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float Chance { get; set; }
        public List<IDamageModification> Stacks { get; set; }
        public StackType StackType { get; set; }
        public bool CanEvade { get; set; }
        public bool CanIgnoreArmor { get; set; }
        public bool CanIgnoreBlock { get; set; }

        public void Reduce(DamageInfo damageInfo)
        {

        }
    }
}
