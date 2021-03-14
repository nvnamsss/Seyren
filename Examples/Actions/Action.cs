using UnityEngine;
using System.Collections.Generic;
using Seyren.System.Actions;
using System.Threading.Tasks;
using Seyren.System.Units;
using System.Threading;

namespace Seyren.Examples.Actions
{
    [DisallowMultipleComponent]
    public class Action : MonoBehaviour
    {
        public delegate void Do();
        public static readonly IAction Free = new FreeAction();
        public ActionType Type { get; set; }
        public IAction CurrentAction => _currentAction;
        public Animator animator;
        private DelayAction delayAction;
        private Queue<IAction> _queue;
        private volatile IAction _currentAction;
        Action()
        {
            _currentAction = Free;
        }
        private void Awake()
        {
            animator = GetComponent<Animator>();
            _queue = new Queue<IAction>();
        }

        private void FixedUpdate()
        {
        }

        public void DoAction(IAction action, params object[] obj) {
            // check for the constraint
            if (CurrentAction.Constraint(action)) return;

            // do the action
            _currentAction = action;
            foreach (IThing thing in action.Do(obj))
            {
                thing.Do(obj);                
            }
        }

    }

    public class DelayThing : IThing
    {
        int delay;

        public DelayThing(int delay) {
            this.delay = delay;
        }


        public async void Do(params object[] obj)
        {
            await Task.Delay(delay);
        }
    }

    public class AnimationThing : IThing
    {
        string name;
        private Animator animator;
        public AnimationThing(string name) {
            this.name = name;
        }

        public void Do(params object[] obj)
        {
            Action a = (obj[0] as Action);
            a.animator.Play(name);
        }
    }

    public class DoThing : IThing
    {
        public delegate void ThingToDo();
        ThingToDo action;

        public DoThing(ThingToDo action) {
            this.action = action;
        }

        public void Do(params object[] obj)
        {
            action();
        }
    }

    
    public class OrderMovingThing : IThing
    {
        Unit unit;
        Unit target;

        public OrderMovingThing(Unit unit, Unit target) {
            this.unit = unit;
            this.target = target;
        }

        public void Do(params object[] obj)
        {
            // order a unit to reach a target in range
        }
    }
}
