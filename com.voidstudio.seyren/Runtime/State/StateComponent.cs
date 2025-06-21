using UnityEngine;

namespace Seyren.State
{
    public class StateComponent : MonoBehaviour
    {
        private StateMachine _stateMachine;
        
        public StateMachine StateMachine => _stateMachine;
        public IState CurrentState => _stateMachine?.CurrentState;

        protected virtual void Awake()
        {
            _stateMachine = new StateMachine();
            InitializeStates();
        }

        protected virtual void InitializeStates()
        {
            // Override in derived classes to register states
        }

        protected virtual void Update()
        {
            _stateMachine?.Update();
        }

        protected virtual void OnDestroy()
        {
            _stateMachine?.Clear();
        }

        public bool ChangeState<T>() where T : IState
        {
            return _stateMachine?.ChangeState<T>() ?? false;
        }

        public bool ChangeState(IState newState)
        {
            return _stateMachine?.ChangeState(newState) ?? false;
        }

        public void RegisterState<T>(T state) where T : IState
        {
            _stateMachine?.RegisterState(state);
        }

        public T GetState<T>() where T : class, IState
        {
            return _stateMachine?.GetState<T>();
        }
    }
}
