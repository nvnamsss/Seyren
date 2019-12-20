using Base2D.Init.DamageModification;
using Base2D.System.AbilitySystem;
using Base2D.System.ActionSystem;
using Base2D.System.DamageSystem;
using Base2D.System.DamageSystem.Critical;
using Base2D.System.DamageSystem.Evasion;
using Base2D.System.ForceSystem;
using Base2D.System.UnitSystem.EventData;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Base2D.System.UnitSystem.Units
{
    public partial class Unit : MonoBehaviour, IObject, IAttribute
    {
        public delegate void DyingHandler(Unit sender, UnitDyingEventArgs e);
        public delegate void DiedHandler(Unit sender, UnitDiedEventArgs e);
        public delegate void TakeDamageHandler(Unit sender, TakeDamageEventArgs e);
        public delegate void StateChangedHandler(Unit sender, StateChangedEventArgs e);
        public delegate void StatusChangedHandler(Unit sender, StatusChangedEventArgs e);
        public event StateChangedHandler StateChanged;
        public event StatusChangedHandler StatusChanged;
        public event DyingHandler Dying;
        public event DiedHandler Died;
        public event TakeDamageHandler TakeDamage;

        public Player Player { get; set; }
        public int CustomValue { get; set; }
        public bool Targetable { get; set; }
        public bool Invulnerable { get; set; }
        public float Size { get; set; }
        public float Height { get; set; }
        public float AnimationSpeed { get; set; }
        public float TurnSpeed { get; set; }
        public Color VertexColor { get; set; }
        public Unit Owner { get; set; }
        public ModificationInfos Modification { get; set; }
        public IAttachable Attach { get; set; }
        public Rigidbody2D Body { get; set; }
        public Attribute Attribute { get; set; }
        public Dictionary<string, Sprite> Sprites { get; set; }
        public Dictionary<int, Ability> Abilites { get; set; }
        public List<Action> Actions { get; set; }
        public float TimeScale;
        public UnitStatus UnitStatus
        {
            get
            {
                return _unitStatus;
            }
            set
            {
                _unitStatus = value;
            }
        }
        public bool IsFly
        {
            get
            {
                return _isFly;
            }
            set
            {
                _isFly = value;
            }
        }
        public float CurrentHp
        {
            get
            {
                return _currentHp;
            }
            set
            {
                StateChangedHandler state = StateChanged;
                StateChangedEventArgs sce = new StateChangedEventArgs(UnitState.Hp, _currentHp, value);
                if (state != null)
                {
                    state.Invoke(this, sce);
                }

                _currentHp = sce.NewValue;
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
                StateChangedHandler state = StateChanged;
                StateChangedEventArgs sce = new StateChangedEventArgs(UnitState.Mp, _currentMp, value);
                if (state != null)
                {
                    state.Invoke(this, sce);
                }

                _currentMp = sce.NewValue;
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
                StateChangedHandler state = StateChanged;
                StateChangedEventArgs sce = new StateChangedEventArgs(UnitState.Shield, _currentShield, value);
                if (state != null)
                {
                    state.Invoke(this, sce);
                }

                _currentShield = sce.NewValue;
            }
        }

        public float CurrentMShield
        {
            get
            {
                return CurrentMShield;
            }
            set
            {
                StateChangedHandler state = StateChanged;
                StateChangedEventArgs sce = new StateChangedEventArgs(UnitState.MagicalShield, CurrentMShield, value);
                if (state != null)
                {
                    state.Invoke(this, sce);
                }

                CurrentMShield = sce.NewValue;
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
                StateChangedHandler state = StateChanged;
                StateChangedEventArgs sce = new StateChangedEventArgs(UnitState.PhysicalShield, _currentPShield, value);
                if (state != null)
                {
                    state.Invoke(this, sce);
                }

                _currentPShield = sce.NewValue;
            }
        }

        public int JumpTimes
        {
            get
            {
                return _jumpTimes;
            }
            set
            {
                _jumpTimes = value;
            }
        }
        protected Unit _owner;
        [SerializeField]
        protected bool _isFly;
        [SerializeField]
        protected float _currentShield;
        [SerializeField]
        protected float _currentMShield;
        [SerializeField]
        protected float _currentPShield;
        [SerializeField]
        protected float _currentHp;
        [SerializeField]
        protected float _currentMp;
        [SerializeField]
        protected int _jumpTimes;
        [SerializeField]
        protected UnitStatus _unitStatus;
    }
}
