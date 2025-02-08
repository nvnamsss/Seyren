using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Damages.Reduce
{
    public class ReductionInfos : DamageModifications<ReductionInfo>, IEnumerable
    {
        public ReductionInfos()
        {
            _modification = new Dictionary<int, ReductionInfo>();
        }

    }
}
