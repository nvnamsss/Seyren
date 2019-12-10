using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.UnitSystem.EventData
{
    public class StateChangedEventArgs
    {
        public UnitState State { get; set; }
        public float OldValue { get; set; }
        public float NewValue { get; set; }

        public StateChangedEventArgs(UnitState state, float oldValue, float newValue)
        {
            State = state;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
