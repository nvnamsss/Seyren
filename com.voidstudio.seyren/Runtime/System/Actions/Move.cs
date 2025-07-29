using Seyren.System.Common;
using Seyren.System.States;
using Seyren.System.Units;
using Seyren.Universe;
using UnityEngine;
using System;

namespace Seyren.System.Actions
{
    public class Move : IAction
    {
        public int ActionType => 1;

        public bool IsCompleted => completed;

        public string ID => id;

        public bool IsStarted => started;

        private string id;
        private bool completed;
        private bool started;

        public event GameEventHandler<IAction> OnStarted;
        public event GameEventHandler<IAction> OnStopped;
        public event GameEventHandler<IAction> OnCompleted;
        private IUnit unit;
        private Vector3 target;

        public Move(IUnit unit, Vector3 target)
        {
            this.unit = unit;
            this.target = target;
            this.id = Guid.NewGuid().ToString();
            this.completed = false;
            this.started = false;
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
            started = true;
        }

        public void Stop()
        {
            onComplete();
        }

        private void onComplete()
        {
            completed = true;
            OnCompleted?.Invoke(this);
        }
    }
}