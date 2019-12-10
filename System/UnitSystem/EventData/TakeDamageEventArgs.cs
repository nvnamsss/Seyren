using Crom.System.DamageSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.UnitSystem.EventData
{
    public class TakeDamageEventArgs
    {
        DamageInfo Info { get; }
        public TakeDamageEventArgs(DamageInfo info)
        {
            Info = info;
        }
    }
}
