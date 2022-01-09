using System;
using Seyren.System.Abilities;
using Seyren.System.Actions;
using Seyren.System.Damages;
using Seyren.System.Forces;
using Seyren.System.Generics;
using Seyren.System.States;
using UnityEngine;

namespace Seyren.System.Units
{
    public interface IUnit : IObject
    {
        event GameEventHandler<IUnit, MovedEventArgs> OnMoved;
        /// <summary>
        /// Determine unit is going to die
        /// </summary>
        event GameEventHandler<IUnit, UnitDyingEventArgs> OnDying;
        /// <summary>
        /// Determine unit is killed by some one
        /// </summary>
        event GameEventHandler<IUnit, UnitDiedEventArgs> OnDied;
        event GameEventHandler<IUnit, TakeDamageEventArgs> OnDamaged;


        long UnitID { get; }
        IUnit Owner { get; }
        Force Force { get; }
        State State { get; }
        ActionCollection Actions {get;}
        Modification Modification { get; }
        States.IAttribute Attribute { get;set; }

        Error Kill(IUnit by);
        Error Move(Vector3 location);
        Error Look(Quaternion quaternion);
        Error Damage(DamageInfo damage);
        Error Cast(Ability ability);
    }
}
