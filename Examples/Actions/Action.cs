using UnityEngine;
using System.Collections.Generic;
using Seyren.System.Actions;

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

        public Work DoAction(Do action) {
            // do the action
            delayAction.Invoke();

            // do the animation
            animator.Play("attack", 0);

            // do the real work
            action.Invoke();

            Work work = new Work();
            work.animator = animator;
            work.action = this;

            return work;
        }


        public Work Channel(Do action) {
            return null;
        }

        public void Play(IAction action)
        {
            bool run = action.RunCondition(_currentAction);
            if (run)
            {
                action.ActionEnd += ActionCompleteCallback;
                action.ActionStart += ActionStartCallback;
                action.Invoke();
            }
        }

        private void ActionStartCallback(IAction s)
        {
            _currentAction = s;
            s.ActionStart -= ActionStartCallback;
        }

        private void ActionCompleteCallback(IAction s)
        {
            _currentAction = Free;
            s.ActionEnd -= ActionCompleteCallback;
        }
        //public void Queue(IAction action)
        //{
        //    _queue.Enqueue(action);
        //    enabled = true;
        //}

        //public IAction Dequeue()
        //{
        //    if (_queue.Count > 0)
        //    {
        //        return _queue.Dequeue();
        //    }
        //    else
        //    {
        //        enabled = false;
        //        return Free;
        //    }
        //}
    }
}
