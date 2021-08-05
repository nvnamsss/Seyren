using Seyren.Examples.DamageModification;
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
    public enum StateValue
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
    public partial class Unit : IUnit, IAttribute
    {
        public event GameEventHandler<Unit, UnitMovedEventArgs> Moved; 
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
        public event GameEventHandler<Unit, TakeDamageEventArgs> TookDamage;
        /// <summary>
        /// Determine unit killing another one
        /// </summary>
        public event GameEventHandler<Unit, Unit> Killing;
        public long ID {get;}
        public Player Player { get; set; }
        public Vector3 position;
        public Quaternion rotation;
        public string name;
        public bool Targetable { get; set; }
        public bool Invulnerable { get; set; }
        public float Size { get; set; }
        public float Height { get; set; }
        public float AnimationSpeed { get; set; }
        public float TurnSpeed { get; set; }
        public Unit Owner { get; set; }
        public ModificationInfos Modification { get; set; }
        public IAttachable Attach { get; set; }
        public Dictionary<string, Sprite> Sprites { get; set; }
        // public AbilityCollection Ability { get; set; }
        // public Dictionary<int, Ability> Abilites { get; set; }
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
                StateChangeEventArgs sce = new StateChangeEventArgs(StateValue.Hp, _currentHp, value);
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
                StateChangeEventArgs sce = new StateChangeEventArgs(StateValue.Mp, _currentMp, value);
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
                StateChangeEventArgs sce = new StateChangeEventArgs(StateValue.Shield, _currentShield, value);
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
                StateChangeEventArgs sce = new StateChangeEventArgs(StateValue.MagicalShield, _currentMShield, value);
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
                StateChangeEventArgs sce = new StateChangeEventArgs(StateValue.PhysicalShield, _currentPShield, value);
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

        public bool IsHidden { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsInvulnerable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Vector3 Location => throw new NotImplementedException();

        public Quaternion Rotation => throw new NotImplementedException();

        public Force Force => throw new NotImplementedException();

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

        public bool Kill()
        {
            throw new NotImplementedException();
        }

        public Error Kill(IUnit by)
        {
            throw new NotImplementedException();
        }

        Error IUnit.Move(Vector3 location)
        {
            throw new NotImplementedException();
        }

        Error IUnit.Look(Quaternion quaternion)
        {
            throw new NotImplementedException();
        }

        public Error Damage(DamageInfo damage)
        {
            throw new NotImplementedException();
        }

        Error IObject.Kill()
        {
            throw new NotImplementedException();
        }
    }

}
