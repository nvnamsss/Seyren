using System.Collections.Generic;
using Seyren.System.Common;
using Seyren.System.Units;
using Seyren.Universe;
using UnityEngine;

namespace Seyren.System.Abilities.Common
{
    public class ChainLightningInstance : IAbilityInstance
    {
        public IUnit Caster { get; }
        public float AliveTime { get; private set; }
        public bool IsActive { get; private set; } = true;

        public IUnit currentUnit;
        public Dictionary<string, IUnit> hitUnits;

        public ChainLightningInstance(IUnit caster, Vector3? location, IUnit targetUnit)
        {
            Caster = caster;
        }

        public void Tick(float deltaTime) { AliveTime += deltaTime; }

        public void Cancel() { IsActive = false; }
    }

    public class ChainLightning : Ability
    {
        public ChainLightning(Universe.Universe universe) : base(universe)
        {
        }

        public override (IAbilityInstance instance, Error error) Cast(AbilityData data)
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