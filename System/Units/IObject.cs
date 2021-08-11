using System.Collections;
using System.Collections.Generic;
using Seyren.System.Generics;
using UnityEngine;

namespace Seyren.System.Units
{
    public interface ICoordinate {
        Vector3 Location { get; }
        Vector3 Size { get; set; }
        Quaternion Rotation { get; }
    }

    public interface IObject : ICoordinate
    {
        bool IsHidden { get; set; }
        bool IsInvulnerable { get; set; }
        Error Kill();
    }
}

