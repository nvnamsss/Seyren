using Base2D.System.DamageSystem;
using Base2D.System.UnitSystem.Projectiles;
using Base2D.System.UnitSystem.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Base2D.System.UnitSystem.Dummies
{
    public partial class Dummy : MonoBehaviour, IAttribute, IObject
    {
        public delegate void UnitAffectedHandler(Dummy sender, Unit affected);
        public delegate void DummyAffectedHandler(Dummy sender, Dummy affected);
        public delegate void ProjectileAffectedHandler(Dummy sender, Projectile affected);
        public event UnitAffectedHandler UnitIn;
        public event UnitAffectedHandler UnitOut;
        public event DummyAffectedHandler DummyIn;
        public event DummyAffectedHandler DummyOut;
        public event ProjectileAffectedHandler ProjectileIn;
        public event ProjectileAffectedHandler ProjectileOut;

        public int CustomValue { get; set; }
        public bool Targetable { get; set; }
        public bool Invulnerable { get; set; }

        public bool IsFly { get; set; }
        public float Size { get; set; }
        public float Height { get; set; }
        public float AnimationSpeed { get; set; }
        public float TurnSpeed { get; set; }
        public Color VertexColor { get; set; }

        public Attribute Attribute { get; set; }

        public ModificationInfos Modification { get; set; }
        public float HitDelay { get; set; }
        public float TimeExpired { get; set; }
        public List<Unit> AffectedUnits;
        public List<Projectile> AffectedProjectiles;
        public List<Dummy> AffectedDummies;
        public Unit Owner { get; set; }
        public Rigidbody2D Body;
        public Collider2D Collider;
    }
}
