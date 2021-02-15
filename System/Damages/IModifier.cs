using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Damages
{
    public interface IModifier
    {
        bool CanCritical { get; set; }
        bool CanEvade { get; set; }
        bool CanReduce { get; set; }
    }
}
