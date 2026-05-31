using System;
using Seyren.System.Units;
using UnityEngine;

namespace Seyren.System.Abilities
{
    /// <summary>
    /// Moves an <see cref="IUnit"/> toward a destination each tick at a constant speed.
    /// Returns <see cref="NodeState.Success"/> once the entity is within
    /// <see cref="_arrivalThreshold"/> units of the destination.
    /// </summary>
    public class MoveNode : IAbilityNode
    {
        private readonly Func<AbilityGraphContext, IUnit> _entitySelector;
        private readonly Func<AbilityGraphContext, Vector3> _destinationSelector;
        private readonly float _speed;
        private readonly float _arrivalThreshold;

        public MoveNode(
            Func<AbilityGraphContext, IUnit> entitySelector,
            Func<AbilityGraphContext, Vector3> destinationSelector,
            float speed,
            float arrivalThreshold = 0.1f)
        {
            _entitySelector = entitySelector;
            _destinationSelector = destinationSelector;
            _speed = speed;
            _arrivalThreshold = arrivalThreshold;
        }

        public NodeState Tick(AbilityGraphContext context, float deltaTime)
        {
            IUnit entity = _entitySelector(context);
            if (entity == null) return NodeState.Failure;

            Vector3 destination = _destinationSelector(context);
            Vector3 current = entity.Location;
            float distance = Vector3.Distance(current, destination);

            if (distance <= _arrivalThreshold)
                return NodeState.Success;

            Vector3 next = Vector3.MoveTowards(current, destination, _speed * deltaTime);
            entity.Move(next);

            return NodeState.Running;
        }
    }
}
