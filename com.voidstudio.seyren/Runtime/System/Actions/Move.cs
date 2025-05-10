using Seyren.System.Common;
using Seyren.System.States;
using Seyren.System.Units;
using Seyren.Universe;
using UnityEditor.Graphs;
using UnityEngine;
using Sys = global::System;

namespace Seyren.System.Actions
{
    public class Move : IAction
    {
        public int ActionType => 1;

        public bool IsCompleted => completed;

        public string ID => id;
        private string id;
        private bool completed;

        public event GameEventHandler<IAction> ActionStart;
        public event GameEventHandler<IAction> ActionBroke;
        public event GameEventHandler<IAction> ActionEnd;
        private IUnit unit;
        private Vector3 target;

        public Move(IUnit unit, Vector3 target)
        {
            this.unit = unit;
            this.target = target;
            this.id = Sys.Guid.NewGuid().ToString();
        }

        public void Loop(ITime time)
        {
            // get the current position of the unit
            Vector3 currentPosition = unit.Location;
            // create a path to the target
            Vector3 path = target - currentPosition;
            float speed = unit.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED).Total;
            // move the unit along the path
            unit.Move(currentPosition + path.normalized * speed * Time.deltaTime);
            // check if the unit has reached the target
            if (Vector3.Distance(currentPosition, target) < 0.1f)
            {
                onComplete();
            }
        }

        public Error RunCondition()
        {
            return Error.None;
        }

        public void Start()
        {
        }

        public void Stop()
        {
            onComplete();
        }

        private void onComplete()
        {
            completed = true;
            ActionEnd?.Invoke(this);
        }
    }
}