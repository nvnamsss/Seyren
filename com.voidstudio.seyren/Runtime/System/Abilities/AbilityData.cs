using UnityEngine;
using Seyren.System.Units;

namespace Seyren.System.Abilities
{
    public class AbilityData
    {
        public int level;
        public IUnit caster;
        public IUnit target;
        public Vector3? location;
        // vector start and end for ability that has a line effect
        public Vector3? start;
        public Vector3? end;
    }

}