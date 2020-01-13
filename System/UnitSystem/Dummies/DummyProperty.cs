using Base2D.System.DamageSystem;
using Base2D.System.UnitSystem.Units;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base2D.System.UnitSystem.Dummies
{
    public partial class Dummy : MonoBehaviour, IAttribute, IObject
    {
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
        public Unit Owner { get; set; }
    }
}
