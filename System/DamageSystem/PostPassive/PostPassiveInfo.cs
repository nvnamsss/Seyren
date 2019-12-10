using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.DamageSystem.PostPassive
{
    public class PostPassiveInfo : IDamageModification
    {
        public static readonly PostPassiveInfo None = new PostPassiveInfo();
        public string Id { get; set; }
        public string Name { get; set; }
        public float Chance { get; set; }
        public bool CanStack { get; set; }
        public List<IDamageModification> Stacks { get; set; }
        public StackType StackType { get; set; }
    }
}
