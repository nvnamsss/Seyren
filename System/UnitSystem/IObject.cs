using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base2D.System.UnitSystem
{
    public interface IObject
    {
        bool IsFly { get; set; }
        float AnimationSpeed { get; set; }
        float TurnSpeed { get; set; }
        Color VertexColor { get; set; }
    }
}

