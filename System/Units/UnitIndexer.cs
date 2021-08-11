using Seyren.System.Generics;

namespace Seyren.System.Units
{
    public class UnitIndexer {
        public event GameEventHandler<IUnit> OnUnitCreated;
        public event GameEventHandler<IUnit> OnUnitRemoved;
        public IUnit Create() {
            IUnit u = new Unit();
            OnUnitCreated?.Invoke(u);

            return u;
        }
    }
}