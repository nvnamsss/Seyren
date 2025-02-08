using System.Collections.Concurrent;
using System.Collections.Generic;
using Seyren.System.Generics;
using Seyren.System.Units.Dummies;
using UnityEngine;

namespace Seyren.System.Units
{
    public class UnitIndexer
    {
        public event GameEventHandler<IUnit> OnUnitCreated;
        public event GameEventHandler<IUnit> OnUnitRemoved;

        ConcurrentDictionary<long, Unit> id2unit;
        ConcurrentDictionary<long, Unit> id2dummy;
        ConcurrentQueue<long> unitBin;
        ConcurrentQueue<long> recycleDummy;
        bool recycleWhenUnitDied;
        public UnitIndexer()
        {
            id2unit = new ConcurrentDictionary<long, Unit>();
        }

        public UnitIndexer UseUnit() {
            id2unit = new ConcurrentDictionary<long, Unit>();
            return this;
        }

        public UnitIndexer UseDummy() {
            id2dummy = new ConcurrentDictionary<long, Unit>();
            return this;
        }

        public UnitIndexer RecycleWhenUnitDied() {
            unitBin = new ConcurrentQueue<long>();
            recycleWhenUnitDied = true;
            return this;
        }

        public void AddUnit(IUnit unit) {
            
        }
        public Unit Create()
        {
            Unit unit;
            long uid = 0;

            if (recycleWhenUnitDied && unitBin.Count > 0 && unitBin.TryDequeue(out uid))
            {
                if (id2unit.TryGetValue(uid, out unit))
                {
                    return unit;
                }
            }
            unit = new Unit();

            OnUnitCreated?.Invoke(unit);
            bool ok = id2unit.TryAdd(unit.UnitID, unit);
#if UNITY_EDITOR
            if (!ok)
            {
                Debug.Log("try add to concurrent map failed");
            }
#endif

            return unit;
        }


    }
}