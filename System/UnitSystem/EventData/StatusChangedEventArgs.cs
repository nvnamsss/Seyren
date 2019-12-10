using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.UnitSystem.EventData
{
    public class StatusChangedEventArgs : EventArgs
    {
        public UnitStatus Status { get; }
        public StatusChangedEventArgs(UnitStatus status)
        {
            Status = status;
        }
    }
}
