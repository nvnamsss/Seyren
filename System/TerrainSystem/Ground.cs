using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base2D.System.TerrainSystem
{
    public class Ground : MonoBehaviour
    {
        public static LayerMask Grass = LayerMask.GetMask("GrassGround");
        public static LayerMask Hard = LayerMask.GetMask("HardGround");
        public GroundType GroundType {get; }

        public static GroundType GetGround(string layer)
        {
            GroundType type = GroundType.Unknown;

            return type;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

