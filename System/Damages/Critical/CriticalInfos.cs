using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Seyren.System.Damages.Critical
{
    [Serializable]
    public class CriticalInfos : DamageModifications<CriticalInfo>
    {
        public CriticalInfos()
        {
            _modification = new Dictionary<int, CriticalInfo>();
        }

        public void Trigger(Damage damage) {
            foreach (var critical in _modification.Values)
            {
                critical.Trigger(damage);
            }
        }
    }
}
