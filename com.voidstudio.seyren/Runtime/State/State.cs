using UnityEngine;

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

    public abstract class State : IState
    {
        public string Name { get; protected set; }
        public string ID { get; protected set; }

        protected State(string name)
        {
            Name = name;
        }

        public virtual void Enter()
        {
            Debug.Log($"Entering state: {Name}");
        }

        public virtual void Update()
        {
            // Override in derived classes
        }

        public virtual void Exit()
        {
            Debug.Log($"Exiting state: {Name}");
        }

        public virtual bool CanTransitionTo(IState nextState)
        {
            return true;
        }
    }
}