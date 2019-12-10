using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crom.System.UnitSystem
{
    public interface IObject
    {
        bool IsFly { get; set; }
        float Size { get; set; }
        float Height { get; set; }
        float AnimationSpeed { get; set; }
        float TurnSpeed { get; set; }
        Color VertexColor { get; set; }
    }
}

