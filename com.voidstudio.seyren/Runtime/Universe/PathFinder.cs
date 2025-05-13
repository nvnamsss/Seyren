using System.Collections.Generic;
using Seyren.System.Actions;
using Seyren.System.Units;
using UnityEngine;

namespace Seyren.Universe
{
    public interface IPathFinder
    {
        Vector3[] FindPath(Vector3 start, Vector3 end);
        IAction CreateMoveAction(IUnit unit, Vector3 location);
    }
}