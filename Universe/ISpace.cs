using System.Collections.Generic;
using Seyren.System.Units;
using UnityEngine;

namespace Seyren.Universe
{
    public interface ISpace
    {
        List<IUnit> GetUnits(Vector3 location, float radius);
        
        bool AddUnit(IUnit unit);
        bool RemoveUnit(IUnit unit);

    }
}