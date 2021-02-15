using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Damages.PrePassive
{
    public class PrePassiveInfos : DamageModifications<PrePassiveInfo>
    {
        public PrePassiveInfos()
        {
            _modification = new Dictionary<int, PrePassiveInfo>();
        }
    }
}
