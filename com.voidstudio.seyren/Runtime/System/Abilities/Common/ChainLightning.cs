using System.Collections.Generic;
using Seyren.System.Common;
using Seyren.System.Units;
using Seyren.Universe;
using UnityEngine;

namespace Seyren.System.Abilities.Common
{
    public class ChainLightningInstance : AbilityInstance
    {
        public IUnit currentUnit;
        public Dictionary<string, IUnit> hitUnits;

        public ChainLightningInstance(IUnit caster, Vector3? location, IUnit targetUnit)
        {
            this.caster = caster;
            this.location = location;
            this.targetUnit = targetUnit;
        }
    }

    public class ChainLightning : Ability
    {
        public ChainLightning(Universe.Universe universe) : base(universe)
        {
        }

        public override Error Cast(AbilityData data)
        {
            throw new global::System.NotImplementedException();
        }

        protected override Error Condition(IUnit by)
        {
            throw new global::System.NotImplementedException();
        }

        protected override Error Condition(IUnit by, IUnit target)
        {
            throw new global::System.NotImplementedException();
        }

        protected override Error Condition(IUnit by, Vector3 target)
        {
            throw new global::System.NotImplementedException();
        }

        protected override void onCast()
        {
            throw new global::System.NotImplementedException();
        }

        protected override void TickEffect(ITime time)
        {
            throw new global::System.NotImplementedException();
        }
    }
}