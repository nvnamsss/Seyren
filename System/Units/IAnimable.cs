using System;
using Seyren.System.Damages;
using Seyren.System.Forces;
using Seyren.System.Generics;
using UnityEngine;

namespace Seyren.System.Units {
    public interface IAnimable {
        float AnimationSpeed { get; set; }
        float TurnSpeed { get; set; }
    }
}
