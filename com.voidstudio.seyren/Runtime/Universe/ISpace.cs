using System.Collections.Generic;
using Seyren.System.Units;
using UnityEngine;

namespace Seyren.Universe

{   
    /// <summary>
    /// Represents a space where units can be managed and queried.
    /// This interface provides functionality to add, remove, and retrieve units within a defined spatial area.
    /// </summary>
    /// <remarks>
    /// A space acts as a container for units, allowing spatial operations such as:
    /// - Adding and removing units from the space
    /// - Querying units within a specific radius
    /// - Retrieving specific units by their ID
    /// </remarks>
    public interface ISpace
    {
        
        bool AddUnit(IUnit unit);
        bool RemoveUnit(IUnit unit);
        List<IUnit> GetUnitsInRange(Vector3 location, float radius);
        IUnit GetUnit(string unitID);
    }
}