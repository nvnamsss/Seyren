using Seyren.System.Common;
using Seyren.System.Units;

namespace Seyren.System.States
{
    public class State {
        public GameEventHandler<StateChangeEventArgs> StateChanging;
        public GameEventHandler<StateChangeEventArgs> StateChanged;
        private float _currentHp;
        private float _currentMp;
        private float _currentShield;
        private float _currentMShield;
        private float _currentPShield;

        public float CurrentHp
        {
            get
            {
                return _currentHp;
            }
            set
            {   
                StateChangeEventArgs sce = new StateChangeEventArgs(StateValue.Hp, _currentHp, value);
                StateChanging?.Invoke(sce);
                _currentHp = sce.NewValue;
                StateChanged?.Invoke(sce);
            }
        }
        public float CurrentMp
        {
            get
            {
                return _currentMp;
            }
            set
            {
                StateChangeEventArgs sce = new StateChangeEventArgs(StateValue.Mp,  _currentMp, value);
                StateChanging?.Invoke(sce);
                _currentMp = sce.NewValue;
                StateChanged?.Invoke(sce);
            }
        }

        public float CurrentShield
        {
            get
            {
                return _currentShield;
            }
            set
            {
                StateChangeEventArgs sce = new StateChangeEventArgs(StateValue.Shield,  CurrentShield, value);
                StateChanging?.Invoke(sce);
                _currentShield = sce.NewValue;
                StateChanged?.Invoke(sce);
            }
        }

        public float CurrentMShield
        {
            get
            {
                return _currentMShield;
            }
            set
            {
                StateChangeEventArgs sce = new StateChangeEventArgs(StateValue.MagicalShield,  _currentMShield, value);
                StateChanging?.Invoke(sce);
                _currentMShield = sce.NewValue;
                StateChanging?.Invoke(sce);
            }
        }
        public float CurrentPShield
        {
            get
            {
                return _currentPShield;
            }
            set
            {
                StateChangeEventArgs sce = new StateChangeEventArgs(StateValue.PhysicalShield,  _currentPShield, value);
                StateChanging?.Invoke(sce);
                _currentPShield = sce.NewValue;
                StateChanging?.Invoke(sce);
            }
        }
    }
}