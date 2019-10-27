using Crom.System.DamageSystem;
using Crom.System.DamageSystem.Critical;
using Crom.System.DamageSystem.Evasion;
using Crom.System.DamageSystem.Reduce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.UnitSystem
{
    interface IAttribute
    {
        Attribute Attribute { get; set; }
        ModificationInfos Modification { get; set; }
    }
}
