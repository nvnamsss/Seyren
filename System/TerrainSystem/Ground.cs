using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base2D.System.TerrainSystem
{
    public class Ground : MonoBehaviour
    {
        public static LayerMask Grass; 
        public static LayerMask Hard;
        public GroundType GroundType {get; }

        public static GroundType GetGround(string layer)
        {
            GroundType type = GroundType.Unknown;

            return type;
        }
        // Start is called before the first frame update
        void Awake()
        {
            Grass = LayerMask.GetMask("GrassGround");
            Hard = LayerMask.GetMask("HardGround");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

