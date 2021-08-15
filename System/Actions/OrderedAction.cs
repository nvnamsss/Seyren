using System.Collections.Generic;
using Seyren.System.Generics;

namespace Seyren.System.Actions
{
    /// <summary>
    /// OrderedAction will run the action till Break is called
    /// </summary> 
    public class OrderedAction : IAction
    {

        public int ActionType => actionType;

        public bool IsCompleted => completed;

        public ActionConditionHandler RunCondition { get => throw new global::System.NotImplementedException(); set => throw new global::System.NotImplementedException(); }

        public event GameEventHandler<IAction> ActionStart;
        public event GameEventHandler<IAction> ActionEnd;
        public event GameEventHandler<IAction> ActionBroke;

        int actionType;
        int affectedBy;
        bool completed;
        bool running;

        public OrderedAction(int actionType, List<int> affectedBy)
        {
            this.actionType = actionType;
            for (int i = 0; i < affectedBy.Count; i++)
            {
                this.affectedBy |= affectedBy[i];
            }
        }
        public bool Break()
        {
            running = false;
            completed = true;
            ActionEnd?.Invoke(this);
            return true;
        }

        public Error Constraint(IAction action)
        {
            return null;
        }

        public bool IsAffectedBy(int actionType)
        {
            return false;
        }

        public Error Run()
        {
            running = true;
            completed = false;
            ActionStart?.Invoke(this);
            return null;
        }

        Error IAction.RunCondition()
        {
            if (running) {
                return new Error("action is running");
            }
            return null;
        }
    }
}