using System;
using System.Collections.Generic;
using Seyren.System.Abilities;
using Seyren.System.Actions;
using Seyren.System.Damages;
using Seyren.System.Forces;
using Seyren.System.Common;
using Seyren.System.States;
using Seyren.Universe;
using UnityEngine;

namespace Seyren.System.Units
{
    public enum UnitType
    {
        Enemy,
        Hero,
        Neutral,
        Structure,
        Minion,
    }

    /*
    Unit will manage its stte, its action, its ability, its modification, its attribute, its resistance, its on hit effect
    It will decide how to move, how to look, how to damage, how to cast ability, how to kill, how to die

    */
    public interface IUnit : IObject, ILoop
    {
        // event GameEventHandler<IUnit, MovedEventArgs> OnMoved;
        // /// <summary>
        // /// Determine unit is going to die
        // /// </summary>
        // event GameEventHandler<IUnit, UnitDyingEventArgs> OnDying;
        // /// <summary>
        // /// Determine unit is killed by some one
        // /// </summary>
        // event GameEventHandler<IUnit, UnitDiedEventArgs> OnDied;
        // event GameEventHandler<IUnit, TakeDamageEventArgs> OnDamaged;

        /// <summary>
        /// Unit ID to determint the type of unit
        /// </summary>
        // string UnitID { get; } 
        /// <summary>
        /// Reference ID to determine the instance id of unit
        /// </summary>
        string ReferenceID { get; }

        IAttribute Attribute { get; }
        IUnit Owner { get; }
        Force Force { get; set; }
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
        List<IModifier> GetModifiers();
        List<IResistance> GetResistances();
        List<IOnHitEffect> GetOnHitEffects();
        void InflictDamage(Damage damage);
        bool IsImmune();
        string UnitType { get; }
        ActionQueue ActionQueue { get; }
    }
}
