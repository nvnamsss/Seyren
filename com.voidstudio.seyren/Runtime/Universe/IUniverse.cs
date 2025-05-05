using System;
using Seyren.System.Abilities;
using Seyren.System.Damages;
using Seyren.System.Generics;
using Seyren.System.Units;
using UnityEngine;

namespace Seyren.Universe
{

    // Todo: IUniverse now becomes an abstract class with minimum implementation
    public interface IUniverse
    {
        abstract ITime Time { get; }
        abstract ISpace Space { get; }

        void CallAPI<T1, T2, T3>(T1 p1, T2 p2, T3 p3);
        // This function allow the client to call an API function with a name and parameters
        void CallAPI(string functionName, object[] parameters);

        // Need a function to allow the Universe notify changed to the client
        IUnit SpawnUnit(string unitID, Vector3 location, Quaternion rotation);
        void MoveTo(string unitID, Vector3 location);
    }



    /// <summary>
    /// Represents an abstract base class for universe simulation.
    /// A universe contains fundamental components like time and space, and processes periodic updates through a loop mechanism.
    /// </summary>
    /// <remarks>
    /// The Universe class serves as a foundation for creating different types of universe simulations.
    /// It manages the temporal and spatial aspects through ITime and ISpace interfaces.
    /// The class implements a tick-based update system where the Loop method is called on each time tick.
    /// </remarks>
    /// <seealso cref="ITime"/>
    /// <seealso cref="ISpace"/>
    public abstract class Universe
    {
        // List event here
        public event GameEventHandler<IUnit, Damage> OnUnitTookDamage;
        public event GameEventHandler<IUnit> OnUnitDied;
        public event GameEventHandler<IUnit> OnUnitCreated;

        public ITime Time { get; }
        public ISpace Space { get; }
        public IAbilitySystem AbilitySystem { get; set; }
        public DamageOverTimeManager DamageOverTimeManager { get; set; }

        public Universe(ITime time, ISpace space)
        {
            this.Time = time;
            this.Space = space;
            this.DamageOverTimeManager = new DamageOverTimeManager();
        }

        public void Initialize()
        {
            Time.OnTick += loop;
            DamageEngine.OnInflictedDamage += (damage) =>
            {
                OnUnitTookDamage?.Invoke(damage.Target, damage);
            };
        }

        /*
        Logic goes here
        This function will be called every tick of the time
        */
        /// <summary>
        /// Logic goes here.
        /// This function will be called every tick of the time.
        /// </summary>
        public abstract void Loop();
        private void loop(ITime time)
        {
            Loop();
        }

        /// <summary>
        /// Creates a new unit in the universe at the specified location and rotation.
        /// </summary>
        /// <param name="unitID">The type identifier for the unit</param>
        /// <param name="location">The spawn position of the unit</param>
        /// <param name="rotation">The initial rotation of the unit</param>
        /// <returns>The created unit instance</returns>
        public virtual void CreateUnit(IUnit unit)
        {
            OnUnitCreated?.Invoke(unit);

            Space.AddUnit(unit);
        }

        public virtual void KillUnit(IUnit unit) {
            Space.RemoveUnit(unit);
            OnUnitDied?.Invoke(unit);
            // change state to dead
        }

    }
}