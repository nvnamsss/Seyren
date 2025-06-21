using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.State
{
    public class StateMachine
    {
        private IState _currentState;
        private Dictionary<Type, IState> _states;
        
        public IState CurrentState => _currentState;
        public event Action<IState, IState> OnStateChanged;

        public StateMachine()
        {
            _states = new Dictionary<Type, IState>();
        }

        public void RegisterState<T>(T state) where T : IState
        {
            var stateType = typeof(T);
            if (_states.ContainsKey(stateType))
            {
                Debug.LogWarning($"State {stateType.Name} is already registered.");
                return;
            }
            
            _states[stateType] = state;
        }

        public void UnregisterState<T>() where T : IState
        {
            var stateType = typeof(T);
            if (_states.ContainsKey(stateType))
            {
                if (_currentState?.GetType() == stateType)
                {
                    _currentState?.Exit();
                    _currentState = null;
                }
                _states.Remove(stateType);
            }
        }

        public bool ChangeState<T>() where T : IState
        {
            return ChangeState(typeof(T));
        }

        public bool ChangeState(Type stateType)
        {
            if (!_states.TryGetValue(stateType, out var newState))
            {
                Debug.LogError($"State {stateType.Name} is not registered.");
                return false;
            }

            return ChangeState(newState);
        }

        public bool ChangeState(IState newState)
        {
            if (newState == null)
            {
                Debug.LogError("Cannot change to null state.");
                return false;
            }

            if (_currentState == newState)
            {
                Debug.LogWarning("Already in the requested state.");
                return false;
            }

            if (_currentState != null && !_currentState.CanTransitionTo(newState))
            {
                Debug.LogWarning($"Cannot transition from {_currentState.GetType().Name} to {newState.GetType().Name}");
                return false;
            }

            var previousState = _currentState;
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
            
            OnStateChanged?.Invoke(previousState, _currentState);
            return true;
        }

        public void Update()
        {
            _currentState?.Update();
        }

        public T GetState<T>() where T : class, IState
        {
            var stateType = typeof(T);
            return _states.TryGetValue(stateType, out var state) ? state as T : null;
        }

        public bool HasState<T>() where T : IState
        {
            return _states.ContainsKey(typeof(T));
        }

        public void Clear()
        {
            _currentState?.Exit();
            _currentState = null;
            _states.Clear();
        }
    }
}
