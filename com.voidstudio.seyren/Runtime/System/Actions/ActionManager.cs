using Seyren.Universe;

namespace Seyren.System.Actions
{
    public abstract class ActionManager : ILoop
    {
        public abstract void Add(IAction action);
        public abstract void Remove(string id);
        public abstract void Enqueue(IAction action);
        public abstract IAction Dequeue();
        public abstract IAction Peek();

        public abstract bool IsEmpty();

        public abstract void Loop(ITime time);
        public abstract void Clear();
    }
}