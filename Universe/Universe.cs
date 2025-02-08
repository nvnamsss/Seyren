using System.Collections.Generic;
using Seyren.System.Units;
using UnityEngine;

namespace Seyren.Universe
{
    public class Universe
    {
        // map contains units
        IMap<ICell> map;
        // QuadTree<IUnit> unitTree;
        UnitIndexer unitIndexer;
        public Universe()
        {
        }

        public Universe WithSize(Bounds bounds)
        {
            // unitTree = new QuadTree<IUnit>(bounds);
            // QuadTree<IUnit>.MaxItem = 100;
            return this;
        }

        public Universe WithUnitIndexer(UnitIndexer indexer)
        {
            unitIndexer = indexer;
            indexer.OnUnitCreated += OnUnitCreated;
            indexer.OnUnitRemoved += OnUnitRemoved;

            return this;
        }

        private void OnUnitCreated(IUnit unit)
        {
            unit.OnMoved += UpdateUnitTree;
            // unitTree.AddItem(unit);
        }

        private void OnUnitRemoved(IUnit unit)
        {
            unit.OnMoved -= UpdateUnitTree;
            // unitTree.RemoveItem(unit);
        }

        private void UpdateUnitTree(IUnit unit, MovedEventArgs args)
        {
            // unitTree.UpdateItem(unit, args.OldPosition);
        }

        public List<IUnit> PickUnitsInRange(Vector3 location, float radius)
        {
            List<IUnit> units = new List<IUnit>();
            Bounds b = new Bounds(location, new Vector3(radius,radius,radius));
            // units = unitTree.Search(b, (u) => {
            //     return Vector3.Distance(u.Location, location) - u.Size.magnitude / 2 < radius;
            // });

            return units;
        }
    }
}