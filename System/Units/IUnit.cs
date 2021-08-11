using System;
using Seyren.System.Damages;
using Seyren.System.Forces;
using Seyren.System.Generics;
using UnityEngine;

namespace Seyren.System.Units
{
    public interface IUnit : IObject
    {
        event GameEventHandler<IUnit, UnitMovedEventArgs> OnMoved;
        /// <summary>
        /// Determine unit is going to die
        /// </summary>
        event GameEventHandler<IUnit, UnitDyingEventArgs> OnDying;
        /// <summary>
        /// Determine unit is killed by some one
        /// </summary>
        event GameEventHandler<IUnit, UnitDiedEventArgs> OnDied;
        event GameEventHandler<Unit, TakeDamageEventArgs> OnDamaged;


        long UnitID { get; }
        IUnit Owner { get; }
        Force Force { get; }
        Error Kill(IUnit by);
        Error Move(Vector3 location);
        Error Look(Quaternion quaternion);
        Error Damage(DamageInfo damage);
    }
}
