using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.ActionSystem
{
    public class ActionEventArgs : EventArgs
    {
        public bool Changed => Original != New;
        public ActionType Original;
        public ActionType New;

        public ActionEventArgs(ActionType originalValue, ActionType newValue)
        {
            Original = originalValue;
            New = newValue;
        }
    }
}
