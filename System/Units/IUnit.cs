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
    public enum UnitType {
        Enemy,
        Hero,
        Neutral,
        Structure,
    }

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
        
        /// <summary>
        /// Unit ID to determint the type of unit
        /// </summary>
        string UnitID { get; }
        /// <summary>
        /// Reference ID to determine the instance id of unit
        /// </summary>
        string ReferenceID { get; }

        
        IUnit Owner { get; }
        Force Force { get; }
        // State State { get; }
        // ActionCollection Actions {get;}
        // Modification Modification { get; }
        // States.IAttribute Attribute { get;set; }

        // Error Kill(IUnit by);
        Error Move(Vector3 location);
        Error Look(Quaternion quaternion);
        // Error DamageTarget(Damage damage);
        // Error Cast(Ability ability);
        AbilityV2 GetAbility(string abilityID);
    }
}
