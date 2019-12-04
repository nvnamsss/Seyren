using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.UnitSystem.Units
{
    public static class UnitInterface
    {
        public static bool IsEnemy(Unit unit1, Unit unit2)
        {
            return true;
        }

        public static void Pause(Unit target, bool pause)
        {
            if (pause)
            {
                target.TimeScale = 0;
            }
            else
            {
                target.TimeScale = 1.0f;
            }
        }

        public static UnityEngine.GameObject CreateUnit()
        {
            return null;
        }

        public static void AddAbility(Unit unit)
        {

        }

        public static void RemoveAbility(Unit unit)
        {

        }

        public static void SetMovementSpeed(Unit unit, float speed)
        {

        }

        public static void KillUnit(Unit unit)
        {

        }

        public static void RevivetUnit(Unit unit)
        {

        }
    }
}
