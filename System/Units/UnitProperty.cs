using Seyren.System.Terrains;
using Seyren.System.Damages;
using Seyren.System.Forces;
using UnityEngine;
using Seyren.System.Generics;
using Seyren.System.States;

namespace Seyren.System.Units
{

    public enum UnitStatus
    {
        None,
        Moving,
        Slow,
        Knockback,
        Stun,
        Freeze,
        Burning,
        Shocked,
        Poisoned,
        Stonerize,
        Silence,
        Invulnerable,
        SpellImmunity,
    }
    public partial class Unit : IUnit
    {
        public event GameEventHandler<IUnit, UnitMovedEventArgs> OnMoved;
        public event GameEventHandler<Unit, UnitRotatedEventArgs> Rotated;
        /// <summary>
        /// Determine unit state like Hp, Mp, Shield is changing
        /// </summary>
        public event GameEventHandler<Unit, StateChangeEventArgs> StateChanging;
        /// <summary>
        /// Determine unit is going to die
        /// </summary>
        public event GameEventHandler<Unit, UnitDyingEventArgs> Dying;
        /// <summary>
        /// Determine unit is killed by some one
        /// </summary>
        public event GameEventHandler<Unit, UnitDiedEventArgs> Died;
        /// <summary>
        /// Determine unit is took damage
        /// </summary>
        public event GameEventHandler<IUnit, UnitDyingEventArgs> OnDying;
        public event GameEventHandler<IUnit, UnitDiedEventArgs> OnDied;
        public event GameEventHandler<Unit, TakeDamageEventArgs> OnDamaged;

        public long UnitID { get; }
        public Player Player { get; set; }
        public Quaternion rotation;
        public string name;
        public IAttachable Attach { get; set; }
        public Modification Modification { get; set; }
        public GroundType StandOn;
        // public AbilityCollection Ability { get; set; }
        // public Dictionary<int, Ability> Abilites { get; set; }
        public IAttribute Attribute { get; set; }

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

        public Vector3 Location => _position;

        public Quaternion Rotation => rotation;

        public Force Force { get; }

        public Vector3 Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }

        public IUnit Owner => _owner;
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
        public State State => _state;

        public ObjectStatus ObjectStatus { get => throw new global::System.NotImplementedException(); set => throw new global::System.NotImplementedException(); }

        protected State _state;

        public int status;
        protected Vector3 _position;
        protected Vector3 _size;
        protected Quaternion _rotation;
        [SerializeField]
        protected Attribute _attribute;
        [SerializeField]
        protected Attribute _baseAttribute;
        public IUnit _owner;
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
        protected int _currentJump;
        [SerializeField]
        protected UnitStatus _unitStatus;
        protected Unit damageSource;
        public Modification modification;

    }

}
