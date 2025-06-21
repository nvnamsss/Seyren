using Seyren.State;
using Seyren.System.Units;

namespace Seyren.Units
{
    public class UnitStateMachine : StateMachine
    {

        private IUnit _unit;
        public UnitStateMachine(IUnit unit)
        {
            _unit = unit;
        }


        public IUnit Unit => _unit;
    }

    public class UnitStateIdle : IState
    {
        public string ID => "unit_idle";

        public bool CanTransitionTo(IState nextState)
        {
            throw new global::System.NotImplementedException();
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }
    }

    public class UnitStateMove : IState
    {
        public string ID => "unit_move";

        public bool CanTransitionTo(IState nextState)
        {
            throw new global::System.NotImplementedException();
        }

        public void Enter()
        {
            throw new global::System.NotImplementedException();
        }

        public void Exit()
        {
            throw new global::System.NotImplementedException();
        }

        public void Update()
        {
            throw new global::System.NotImplementedException();
        }
    }

    public class UnitStateAttack : IState
    {
        public string ID => "unit_attack";

        public bool CanTransitionTo(IState nextState)
        {
            throw new global::System.NotImplementedException();
        }

        public void Enter()
        {
            throw new global::System.NotImplementedException();
        }

        public void Exit()
        {
            throw new global::System.NotImplementedException();
        }

        public void Update()
        {
            throw new global::System.NotImplementedException();
        }
    }

    public class UnitStateCastAbility : IState
    {
        public string ID => "unit_cast_ability";

        public bool CanTransitionTo(IState nextState)
        {
            throw new global::System.NotImplementedException();
        }

        public void Enter()
        {
            throw new global::System.NotImplementedException();
        }

        public void Exit()
        {
            throw new global::System.NotImplementedException();
        }

        public void Update()
        {
            throw new global::System.NotImplementedException();
        }
    }

    public class UnitStateStunned : IState
    {
        public string ID => "unit_stunned";

        public bool CanTransitionTo(IState nextState)
        {
            throw new global::System.NotImplementedException();
        }

        public void Enter()
        {
            throw new global::System.NotImplementedException();
        }

        public void Exit()
        {
            throw new global::System.NotImplementedException();
        }

        public void Update()
        {
            throw new global::System.NotImplementedException();
        }
    }

    public class UnitStateChanneling: IState
    {
        public string ID => "unit_channeling";

        public bool CanTransitionTo(IState nextState)
        {
            throw new global::System.NotImplementedException();
        }

        public void Enter()
        {
            throw new global::System.NotImplementedException();
        }

        public void Exit()
        {
            throw new global::System.NotImplementedException();
        }

        public void Update()
        {
            throw new global::System.NotImplementedException();
        }
    }

    public class UnitStateDie : IState
    {
        public string ID => "unit_die";

        public bool CanTransitionTo(IState nextState)
        {
            return false;
        }

        public void Enter()
        {
            throw new global::System.NotImplementedException();
        }

        public void Exit()
        {
            throw new global::System.NotImplementedException();
        }

        public void Update()
        {
            throw new global::System.NotImplementedException();
        }
    }
    
}