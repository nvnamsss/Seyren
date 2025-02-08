using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seyren.System.Actions;
using Seyren.System.Generics;

namespace Seyren.Examples.Actions
{
    /// <summary>
    /// An action to free an unit,
    /// unit will do nothing when invoked this action
    /// </summary>
    public class FreeAction : IAction
    {
        public ActionConditionHandler RunCondition { get; }

        public ActionType ActionType { get; }

        int IAction.ActionType => throw new NotImplementedException();

        public bool IsCompleted => throw new NotImplementedException();

        public FreeAction()
        {
            ActionType = ActionType.None;
        }

        public event GameEventHandler<IAction> ActionStart;
        public event GameEventHandler<IAction> ActionEnd;
        public event GameEventHandler<IAction> ActionBroke;

        public void Invoke()
        {
            ActionStart?.Invoke(this);
        }

        public void Revoke()
        {
            ActionEnd?.Invoke(this);
        }

        public bool Break()
        {
            return true;
        }

        public Error Constraint(IAction action)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IThing> Do(params object[] obj)
        {
            throw new NotImplementedException();
        }

        public bool IsAffectedBy(int actionType)
        {
            throw new NotImplementedException();
        }

        public Error Run()
        {
            throw new NotImplementedException();
        }

        Error IAction.RunCondition()
        {
            throw new NotImplementedException();
        }
    }
}
