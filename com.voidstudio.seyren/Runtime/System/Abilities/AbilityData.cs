using UnityEngine;
using Seyren.System.Units;

namespace Seyren.System.Abilities
{
    public class AbilityData
    {
        public string ID { get; set; }

        public IUnit Source { get; set; }

        public IUnit Target { get; set; }

        public Vector3? Location { get; set; }

        public Vector3? VectorStart { get; set; }
        public Vector3? VectorEnd { get; set; }
    }

}