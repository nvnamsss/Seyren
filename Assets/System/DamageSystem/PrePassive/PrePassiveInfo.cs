using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.DamageSystem.PrePassive
{
    public class PrePassiveInfo : IDamageModification
    {
        public static readonly PrePassiveInfo None = new PrePassiveInfo();
        public string Id { get; set; }
        public string Name { get; set; }
        public float Chance { get; set; }
        public bool CanStack { get; set; }
        public List<IDamageModification> Stacks { get; set; }
        public StackType StackType { get; set; }

        public PrePassiveInfo()
        {
            Stacks = new List<IDamageModification>();
        }
    }
}
