using Seyren.System.Units;
using System.Collections.Generic;

namespace Seyren.System.Abilities
{
    public class PassiveAbilityInstance
    {
        public IUnit unit;
    }

    public abstract class PassiveAbility : Ability
    {
        protected Dictionary<IUnit, PassiveAbilityInstance> instances = new Dictionary<IUnit, PassiveAbilityInstance>();

        public PassiveAbility(Universe.Universe universe) : base(universe)
        {
            this.universe = universe;
        }

        public void AddInstance(IUnit unit)
        {
            if (instances.ContainsKey(unit))
                return;

            var instance = new PassiveAbilityInstance { unit = unit };
            instances[unit] = instance;
            onActive(unit);
        }

        public void RemoveInstance(IUnit unit)
        {
            if (!instances.ContainsKey(unit))
                return;

            onDeactive(unit);
            instances.Remove(unit);
        }

        protected abstract void onActive(IUnit unit);
        protected abstract void onDeactive(IUnit unit);

    }


}