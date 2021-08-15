using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seyren.System.Generics;

namespace Seyren.System.Actions
{
    public class ActionPipeline : IAction
    {

        public int ActionType => throw new NotImplementedException();

        public bool IsCompleted => throw new NotImplementedException();

        public ActionConditionHandler RunCondition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event GameEventHandler<IAction> ActionStart;
        public event GameEventHandler<IAction> ActionEnd;
        public event GameEventHandler<IAction> ActionBroke;

        bool broke;
        public ActionPipeline() {
        }

        public bool Break()
        {
            broke = true;
            return broke;
        }

        public Error Constraint(IAction action)
        {
            return null;
        }

        public Error Run()
        {
            throw new NotImplementedException();
        }

        bool IAction.IsAffectedBy(int actionType)
        {
            throw new NotImplementedException();
        }

        Error IAction.RunCondition()
        {
            throw new NotImplementedException();
        }
    }
}
