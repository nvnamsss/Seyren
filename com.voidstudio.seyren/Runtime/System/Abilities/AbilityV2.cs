using Seyren.System.Units;
using Seyren.Universe;

namespace Seyren.System.Abilities {
    public abstract class AbilityV2 {
        public abstract IUnit Caster { get; }
        public abstract bool IsDone { get; }
        // public abstract bool ID { get; }
        // public abstract bool InstanceID { get; }
        public abstract void Process(IUniverse universe);
        public abstract void End();

        // Returns the id of the ability
        public abstract string GetID();
        // Returns the id of current instance
        public abstract string GetInstanceID();
    }
}