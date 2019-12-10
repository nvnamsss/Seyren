using Base2D.System.DamageSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.UnitSystem.EventData
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
