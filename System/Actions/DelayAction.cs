using System;
using System.Threading;
using Seyren.System.Generics;

namespace Seyren.System.Actions
{
    public class DelayAction : IAction
    {
        public ActionConditionHandler RunCondition => throw new global::System.NotImplementedException();

        public ActionType ActionType => throw new global::System.NotImplementedException();

        public event GameEventHandler<IAction> ActionStart;
        public event GameEventHandler<IAction> ActionEnd;
        private ManualResetEvent sync;
        public long delay;
        public bool breakable;
        private long delayTo;
        public DelayAction(long delay, bool breakable) {
            this.breakable = breakable;
        }

        public bool Break()
        {
            if (!breakable) return false;

            delayTo = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            return true;
        }

        public void Invoke()
        {
            if (delayTo < DateTimeOffset.Now.ToUnixTimeMilliseconds()) {
                return;
            }

            delayTo = DateTimeOffset.Now.ToUnixTimeMilliseconds() + 10;
            sync.Set();
            // spin here
            while (true) {
                if (delayTo >= DateTimeOffset.Now.ToUnixTimeMilliseconds()) {
                    break;
                }
            }

            sync.Reset();
        }
        public void Constraint(IAction action)
        {
            throw new NotImplementedException();
        }
    }
}