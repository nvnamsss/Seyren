using System.Collections;
using System.Collections.Generic;
using Seyren.System.Generics;
using UnityEngine;

namespace Seyren.System.Units
{
    public interface IObject
    {
        bool IsHidden { get; set; }
        bool IsInvulnerable {get; set;}
        Vector3 Location { get; }

        Quaternion Rotation {get;}
        Error Kill();
    }
}

