using Seyren.System.Damages;
using Seyren.System.Damages.Critical;
using Seyren.System.Damages.Evasion;
using Seyren.System.Damages.Reduce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Units
{
    public interface IAttribute
    {
        Attribute Attribute { get; set; }
        ModificationInfos Modification { get; set; }
    }
}
