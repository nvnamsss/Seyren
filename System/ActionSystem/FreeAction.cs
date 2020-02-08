using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.ActionSystem
{
    /// <summary>
    /// An action to free an unit,
    /// unit will do nothing when invoked this action
    /// </summary>
    public class FreeAction : IAction
    {
        public ActionConditionHandler RunCondition { get; }

        public ActionType ActionType { get; }

        public event ActionHandler ActionStart;
        public event ActionHandler ActionEnd;

        public FreeAction()
        {
            ActionType = ActionType.None;
        }

        public void Invoke()
        {
            ActionStart?.Invoke(this);
        }

        public void Revoke()
        {
            ActionEnd?.Invoke(this);
        }
    }
}
