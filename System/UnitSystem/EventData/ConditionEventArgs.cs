using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.UnitSystem.EventData
{
    public class ConditionEventArgs<T> : EventArgs
    {
        public T Object { get; }
        public bool Match { get; set; }
        public ConditionEventArgs(T obj, bool match)
        {
            Object = obj;
            Match = match;
        }
    }
}
