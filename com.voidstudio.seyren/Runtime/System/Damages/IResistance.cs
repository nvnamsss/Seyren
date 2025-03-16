using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Damages
{
    public interface IResistance
    {
        void Apply(Damage damage);
    }
}
