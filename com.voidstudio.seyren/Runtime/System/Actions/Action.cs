using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Seyren.System.Generics;

namespace Seyren.System.Actions
{
    public class Action : IAction
    {
        // IThing thing;
        int runtime;
        bool running;
        Task task;
        CancellationTokenSource tokenSource;
        int actionType;
        int affectedBy;
        bool completed;
        public Action(int actionType, int runtime, List<int> affectedBy)
        {
            this.actionType = actionType;
            this.runtime = runtime;
            for (int i = 0; i < affectedBy.Count; i++)
            {
                this.affectedBy |= affectedBy[i];
            }
            tokenSource = new CancellationTokenSource();
        }

        public int ActionType => actionType;

        public bool IsCompleted => completed;

        public event GameEventHandler<IAction> ActionStart;
        public event GameEventHandler<IAction> ActionEnd;
        public event GameEventHandler<IAction> ActionBroke;

        public bool IsAffectedBy(int actionType)
        {
            return (affectedBy | actionType) == affectedBy;
        }

        public bool Break()
        {
            if (!running) return false;
            tokenSource.Cancel();
            ActionBroke?.Invoke(this);
            return true;
        }

        public Error Constraint(IAction action)
        {
            return null;
        }

        public Error Run()
        {
            running = true;
            ActionStart?.Invoke(this);
            task = Task.Run(async () =>
            {
                await Task.Delay(runtime);
                completed = false;
                ActionEnd?.Invoke(this);
            }, tokenSource.Token);
            return null;
        }

        public Error RunCondition()
        {
            if (running) {
                return new Error("action is running");
            }
            return null;
        }
    }
}
