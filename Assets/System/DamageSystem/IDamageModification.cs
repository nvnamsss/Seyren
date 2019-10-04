using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.DamageSystem
{
    public interface IDamageModification
    {
        string Id { get; set; }
        string Name { get; set; }
        float Chance { get; set; }
        List<IDamageModification> Stacks { get; set; }
        StackType StackType { get; set; }
    }
}
