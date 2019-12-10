using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.DamageSystem
{
    public interface IModifier
    {
        bool CanCritical { get; set; }
        bool CanEvade { get; set; }
        bool CanReduce { get; set; }
    }
}
