using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.UnitSystem.EventData
{
    public class UnitDiedEventArgs
    {
        public Unit Killer { get; }

        public UnitDiedEventArgs(Unit killer)
        {
            Killer = killer;
        }
    }
}
