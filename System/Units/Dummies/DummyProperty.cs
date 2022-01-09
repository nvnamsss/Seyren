using Seyren.System.Abilities;
using Seyren.System.Actions;
using Seyren.System.Damages;
using Seyren.System.Forces;
using Seyren.System.Generics;
using Seyren.System.States;
using Seyren.System.Units;
using Seyren.System.Units.Projectiles;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.System.Units.Dummies
{
    public partial class Dummy : MonoBehaviour, IUnit
    {
        public delegate bool AffectConditionHandler<T>(Dummy sender, T obj);
        public delegate void UnitAffectedHandler(Dummy sender, Unit affected);
        public delegate void DummyAffectedHandler(Dummy sender, Dummy affected);
        public delegate void ProjectileAffectedHandler(Dummy sender, Projectile affected);
        public AffectConditionHandler<Unit> UnitCondition;
        public AffectConditionHandler<Dummy> DummyCondition;
        public AffectConditionHandler<Projectile> ProjectileCondition;
        public event UnitAffectedHandler UnitIn;
        public event UnitAffectedHandler UnitOut;
        public event DummyAffectedHandler DummyIn;
        public event DummyAffectedHandler DummyOut;
        public event ProjectileAffectedHandler ProjectileIn;
        public event ProjectileAffectedHandler ProjectileOut;
        public event GameEventHandler<IUnit, Vector3> OnMoved;
        public event GameEventHandler<IUnit, UnitDyingEventArgs> OnDying;
        public event GameEventHandler<IUnit, UnitDiedEventArgs> OnDied;
        public event GameEventHandler<Unit, TakeDamageEventArgs> OnDamaged;

        public int CustomValue { get; set; }
        public bool Targetable { get; set; }
        public bool Invulnerable { get; set; }

        public bool IsFly { get; set; }
        public Vector3 Size { get; set; }
        public float Height { get; set; }
        public float AnimationSpeed { get; set; }
        public float TurnSpeed { get; set; }
        public Color VertexColor { get; set; }
        public bool Active
        {
            get => _active;
            set => _active = value;
        }
        public Vector2 Forward
        {
            get => _forward;
            set => _forward = value;
        }
        public Attribute Attribute { get; set; }

        public Modification Modification { get; set; }
        public float HitDelay { get; set; }
        public float TimeExpired { get; set; }
        public List<Unit> AffectedUnits;
        public List<Projectile> AffectedProjectiles;
        public List<Dummy> AffectedDummies;
        public Unit Owner { get; set; }
        public bool IsHidden { get => throw new global::System.NotImplementedException(); set => throw new global::System.NotImplementedException(); }
        public bool IsInvulnerable { get => throw new global::System.NotImplementedException(); set => throw new global::System.NotImplementedException(); }

        public Vector3 Location => throw new global::System.NotImplementedException();

        public Quaternion Rotation => throw new global::System.NotImplementedException();

        public long UnitID => throw new global::System.NotImplementedException();

        public Force Force => throw new global::System.NotImplementedException();

        public State State => throw new global::System.NotImplementedException();


        public ObjectStatus ObjectStatus { get => throw new global::System.NotImplementedException(); set => throw new global::System.NotImplementedException(); }
        IAttribute IUnit.Attribute { get => throw new global::System.NotImplementedException(); set => throw new global::System.NotImplementedException(); }

        public ActionCollection Actions => throw new global::System.NotImplementedException();

        public Rigidbody2D Body;
        public Collider2D Collider;
        [SerializeField]
        protected Vector2 _forward;
        [SerializeField]
        protected bool _active;

   
        event GameEventHandler<IUnit, TakeDamageEventArgs> IUnit.OnDamaged
        {
            add
            {
                throw new global::System.NotImplementedException();
            }

            remove
            {
                throw new global::System.NotImplementedException();
            }
        }

        event GameEventHandler<IUnit, MovedEventArgs> IUnit.OnMoved
        {
            add
            {
                throw new global::System.NotImplementedException();
            }

            remove
            {
                throw new global::System.NotImplementedException();
            }
        }

        public bool Kill()
        {
            throw new global::System.NotImplementedException();
        }

        public Error Kill(IUnit by)
        {
            throw new global::System.NotImplementedException();
        }

        public Error Move(Vector3 location)
        {
            throw new global::System.NotImplementedException();
        }

        public Error Look(Quaternion quaternion)
        {
            throw new global::System.NotImplementedException();
        }

        public Error Damage(DamageInfo damage)
        {
            throw new global::System.NotImplementedException();
        }

        public Error Cast(Ability ability)
        {
            throw new global::System.NotImplementedException();
        }
    }
}
