using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.DamageSystem
{
    public interface IModifier
    {
        bool CanCritical { get; set; }
        bool CanEvade { get; set; }
        bool CanReduce { get; set; }
    }
}
