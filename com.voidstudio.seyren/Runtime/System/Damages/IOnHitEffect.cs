using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seyren.System.Units;

namespace Seyren.System.Damages
{
    public interface IOnHitEffect
    {
        void Trigger(IUnit target);
    }
}
