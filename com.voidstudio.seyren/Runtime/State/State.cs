namespace Seyren.State
{
    public interface IState
    {
        string ID { get; }
        void Enter();
        void Update();
        void Exit();
        bool CanTransitionTo(IState nextState);
    }

    public abstract class State<T> : IState
    {
        public string Name { get; protected set; }
        public string ID { get; protected set; }
        protected T owner;

        protected State(string name, T owner)
        {
            Name = name;
            this.owner = owner;
            ID = $"{owner.GetType().Name}.{name}";
        }

        public abstract void Enter();

        public abstract void Update();

        public abstract void Exit();

        public abstract bool CanTransitionTo(IState nextState);
    }
}