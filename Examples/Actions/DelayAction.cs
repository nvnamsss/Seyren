using System;
using System.Collections.Generic;
using System.Threading;
using Seyren.System.Actions;
using Seyren.System.Generics;

namespace Seyren.Examples.Actions
{
    public enum DelayState {
        Delay,
        Done,
        Broken
    }

    public class DelayAction : IAction
    {
        public ActionConditionHandler RunCondition => throw new global::System.NotImplementedException();

        public ActionType ActionType => throw new global::System.NotImplementedException();

        public event GameEventHandler<IAction> ActionStart;
        public event GameEventHandler<IAction> ActionEnd;
        public event GameEventHandler<IAction> ActionBroke;

        private ManualResetEvent sync;
        public DelayState State => state;

        int IAction.ActionType => throw new NotImplementedException();

        public bool IsCompleted => throw new NotImplementedException();

        public long delay;
        public bool breakable;
        private long delayTo;
        private DelayState state;

        public DelayAction(long delay, bool breakable) {
            this.breakable = breakable;
        }

        public bool Break()
        {
            if (!breakable) return false;

            state = DelayState.Broken;
            delayTo = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            return true;
        }

        public void Invoke(long delay) {
            this.delay = delay;
            Invoke();
        }

        public void Invoke()
        {
            if (delayTo < DateTimeOffset.Now.ToUnixTimeMilliseconds()) {
                return;
            }

            state = DelayState.Delay;

            delayTo = DateTimeOffset.Now.ToUnixTimeMilliseconds() + 10;
            sync.Set();
            // spin here
            while (true) {
                if (delayTo >= DateTimeOffset.Now.ToUnixTimeMilliseconds()) {
                    state = DelayState.Done;
                    break;
                }
            }

            sync.Reset();
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