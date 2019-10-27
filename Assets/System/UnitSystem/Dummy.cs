using Crom.System.DamageSystem;
using Crom.System.UnitSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.System.AbilitySystem
{
    public class Dummy : MonoBehaviour, IUnit, IAttribute, IObject
    {
        public int CustomValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Targetable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Invulnerable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Unit Owner { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IAttachable Attach { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        
        public bool IsFly { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Height { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float AnimationSpeed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float TurnSpeed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Color VertexColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Crom.System.UnitSystem.Attribute Attribute { get; set; }

        public ModificationInfos Modification { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Damage(Unit target, DamageType type)
        {
            throw new NotImplementedException();
        }

        public void Damage(Unit target, float damage, DamageType type)
        {
            throw new NotImplementedException();
        }
    }
}
