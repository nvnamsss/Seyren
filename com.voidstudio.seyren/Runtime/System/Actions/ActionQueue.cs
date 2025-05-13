using System.Collections.Generic;
using Seyren.Universe;

namespace Seyren.System.Actions
{
    public abstract class ActionQueue : ILoop
    {
        public abstract void Enqueue(IAction action);
        public abstract IAction Dequeue();
        public abstract IAction Peek();

        public abstract bool IsEmpty();

        public abstract void Loop(ITime time);
        public abstract void Clear();
    }

    /// <summary>
    /// ActionQueue is a queue of actions that can be executed in order.
    /// Only one action can be executed at a time.
    /// </summary>
    public class LabActionQueue : ActionQueue
    {
        private Queue<IAction> queue = new Queue<IAction>();

        public override void Enqueue(IAction action)
        {
            queue.Enqueue(action);
        }

        public override IAction Dequeue()
        {
            return queue.Dequeue();
        }

        public override IAction Peek()
        {
            return queue.Peek();
        }

        public override bool IsEmpty()
        {
            return queue.Count == 0;
        }

        public override void Loop(ITime time)
        {
            while (queue.Count > 0 && queue.Peek().IsCompleted)
            {
                queue.Dequeue();
            }

            if (queue.Count > 0)
            {
                IAction action = queue.Peek();
                if (!action.IsStarted)
                {
                    action.Start();
                }
                
                action.Loop(time);
            }
        }

        public override void Clear()
        {
            while (queue.Count > 0)
            {
                IAction action = queue.Dequeue();
                action.Stop();
            }
        }
    }
}