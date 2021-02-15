using Seyren.Example.DamageModification;
using Seyren.System.Terrains;
using Seyren.System.Abilities;
using Seyren.System.Actions;
using Seyren.System.Damages;
using Seyren.System.Damages.Critical;
using Seyren.System.Damages.Evasion;
using Seyren.System.Forces;
using Seyren.System.Units;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using Seyren.System.Generics;

namespace Seyren.System.Units
{
    public enum UnitState
    {
        Hp,
        Mp,
        Shield,
        MagicalShield,
        PhysicalShield,
    }

    [Flags]
    public enum UnitStatus
    {
        None,
        Slow,
        Knockback,
        Stun,
        Invulnerable,
        SpellImmunity,
    }
    public partial class Unit : MonoBehaviour, IObject, IAttribute
    {
        [Obsolete("DyingHandler is currenly deprecated, use EventHandler<TSender, TEvent> instead")]
        public delegate void DyingHandler(Unit sender, UnitDyingEventArgs e);
        [Obsolete("DyingHandler is currenly deprecated, use EventHandler<TSender, TEvent> instead")]
        public delegate void DiedHandler(Unit sender, UnitDiedEventArgs e);
        [Obsolete("DyingHandler is currenly deprecated, use EventHandler<TSender, TEvent> instead")]
        public delegate void TakeDamageHandler(Unit sender, TakeDamageEventArgs e);
        [Obsolete("DyingHandler is currenly deprecated, use EventHandler<TSender, TEvent> instead")]
        public delegate void StateChangedHandler(Unit sender, StateChangeEventArgs e);
        [Obsolete("DyingHandler is currenly deprecated, use EventHandler<TSender, TEvent> instead")]
        public delegate void StatusChangedHandler(Unit sender, StatusChangedEventArgs e);
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
        public event GameEventHandler<Unit, TakeDamageEventArgs> TookDamage;
        /// <summary>
        /// Determine unit killing another one
        /// </summary>
        public event GameEventHandler<Unit, Unit> Killing;
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
        public Dictionary<string, Sprite> Sprites { get; set; }
        public AbilityCollection Ability { get; set; }
        public Dictionary<int, Ability> Abilites { get; set; }
        public List<Ability> a;
        public Attribute Attribute
        {
            get
            {
                return _attribute;
            }
            set
            {
                _attribute = value;
            }
        }
        public Attribute BaseAttribute
        {
            get
            {
                return _baseAttribute;
            }
            set
            {
                _baseAttribute = Attribute;
            }
        }
        public Actions.Action Action { get; set; }


        public GroundType StandOn;
        public Vector2 Forward
        {
            get => _forward;
            set => _forward = value;
        }
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
                GameEventHandler<Unit, StateChangeEventArgs> state = StateChanging;
                StateChangeEventArgs sce = new StateChangeEventArgs(UnitState.Hp, _currentHp, value);
                if (state != null)
                {
                    state.Invoke(this, sce);
                }

                _currentHp = sce.NewValue;
                if (_currentHp <= 0)
                {
                    Kill(damageSource);
                }
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
                GameEventHandler<Unit, StateChangeEventArgs> state = StateChanging;
                StateChangeEventArgs sce = new StateChangeEventArgs(UnitState.Mp, _currentMp, value);
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
                GameEventHandler<Unit, StateChangeEventArgs> state = StateChanging;
                StateChangeEventArgs sce = new StateChangeEventArgs(UnitState.Shield, _currentShield, value);
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
                return _currentMShield;
            }
            set
            {
                GameEventHandler<Unit, StateChangeEventArgs> state = StateChanging;
                StateChangeEventArgs sce = new StateChangeEventArgs(UnitState.MagicalShield, _currentMShield, value);
                if (state != null)
                {
                    state.Invoke(this, sce);
                }

                _currentMShield = sce.NewValue;
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
                GameEventHandler<Unit, StateChangeEventArgs> state = StateChanging;
                StateChangeEventArgs sce = new StateChangeEventArgs(UnitState.PhysicalShield, _currentPShield, value);
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
        public Rigidbody2D Body;
        public Collider2D Collider;
        public bool Active;
        public float TimeScale;
        [SerializeField]
        protected Attribute _attribute;
        [SerializeField]
        protected Attribute _baseAttribute;
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
        protected int _currentJump;
        [SerializeField]
        protected UnitStatus _unitStatus;
        [SerializeField]
        protected Vector2 _forward;
        protected Unit damageSource;
        public ModificationInfos info;
    }

}
