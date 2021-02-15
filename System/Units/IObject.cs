using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.System.Units
{
    public interface IObject
    {
        bool IsFly { get; set; }
        float AnimationSpeed { get; set; }
        float TurnSpeed { get; set; }
        Color VertexColor { get; set; }
    }
}

