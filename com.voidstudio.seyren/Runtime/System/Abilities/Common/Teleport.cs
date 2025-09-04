using System.Collections.Generic;
using Seyren.System.Common;
using Seyren.System.Units;
using Seyren.Universe;
using UnityEngine;

namespace Seyren.System.Abilities.Common
{
    public class TeleportInstance : PhaseAbilityInstance
    {

        public TeleportInstance(IUnit caster, Vector3 location)
        {
            this.caster = caster;
            this.location = location;
        }
    }

    public class TeleportAbility : PhaseAbility<TeleportInstance>
    {
        List<TeleportInstance> instances = new List<TeleportInstance>();
        public TeleportAbility(Universe.Universe universe) : base(universe)
        {
        }

        public override Error Cast(AbilityData data)
        {
            Vector3 targetLocation = data.location ?? data.target.Location;
            TeleportInstance instance = new TeleportInstance(data.caster, targetLocation);
            instances.Add(instance);
            return Error.None;
        }

        protected override void CompletePhase(TeleportInstance instance, PhaseState phaseState)
        {
        }

        protected override Error Condition(IUnit by)
        {
            return Error.None;
        }

        protected override Error Condition(IUnit by, IUnit target)
        {
            return Error.None;
        }

        protected override Error Condition(IUnit by, Vector3 target)
        {
            return Error.None;
        }

        protected override void DefinePhases()
        {
        }

        protected override void onAllPhasesComplete(TeleportInstance instance)
        {
        }

        protected override void onCast()
        {
        }

        protected override void TickEffect(ITime time)
        {
        }

        protected override void UpdatePhase(TeleportInstance instance, PhaseState phaseState, ITime time)
        {
        }
    }
}