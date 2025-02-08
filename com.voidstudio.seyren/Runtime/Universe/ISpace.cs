using System.Collections.Generic;
using Seyren.System.Units;
using UnityEngine;

namespace Seyren.Universe
{
    public interface ISpace
    {
        
        bool AddUnit(IUnit unit);
        bool RemoveUnit(IUnit unit);
        List<IUnit> GetUnits(Vector3 location, float radius);
        IUnit GetUnit(string unitID);
    }
}