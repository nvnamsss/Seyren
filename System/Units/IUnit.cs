using System;
using Seyren.System.Damages;
using Seyren.System.Forces;
using Seyren.System.Generics;
using UnityEngine;

namespace Seyren.System.Units {
    public interface IUnit : IObject{
        long ID {get;}
        Force Force {get;}
        Error Kill(IUnit by);
        Error Move(Vector3 location);
        Error Look(Quaternion quaternion);
        Error Damage(DamageInfo damage);
    }
}
