using Seyren.System.Units;
using UnityEngine;

namespace Seyren.Universe {
    public interface IUniverse {
        abstract ITime Time { get; }
        abstract ISpace Space {get;}

        void CallAPI<T1, T2, T3>(T1 p1, T2 p2, T3 p3);
        // This function allow the client to call an API function with a name and parameters
        void CallAPI(string functionName, object[] parameters);

        // Need a function to allow the Universe notify changed to the client
        IUnit SpawnUnit(string unitID, Vector3 location, Quaternion rotation);
        void MoveTo(string unitID, Vector3 location);
    }
}