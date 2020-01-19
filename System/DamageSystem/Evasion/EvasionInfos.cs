using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.DamageSystem.Evasion
{
    public class EvasionInfos : DamageModifications<EvasionInfo>
    {
        public EvasionInfos()
        {
            _modification = new Dictionary<int, EvasionInfo>();
        }

    }
}
