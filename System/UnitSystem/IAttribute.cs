using Base2D.System.DamageSystem;
using Base2D.System.DamageSystem.Critical;
using Base2D.System.DamageSystem.Evasion;
using Base2D.System.DamageSystem.Reduce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.UnitSystem
{
    public interface IAttribute
    {
        Attribute Attribute { get; set; }
        ModificationInfos Modification { get; set; }
    }
}
