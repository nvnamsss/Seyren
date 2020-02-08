using Base2D.System.ActionSystem.DelayAction;
using Base2D.System.ActionSystem.BreakAtion;
using UnityEngine;
using System.Collections.Generic;

namespace Base2D.System.ActionSystem
{
    [DisallowMultipleComponent]
    public class Action : MonoBehaviour
    {
        public static readonly IAction Free = new FreeAction();
        public struct ActionData
        {
            public string name;
            public float time;
            public bool breakable;
            public IAction action;

            public ActionData(string name, float time, bool breakable, IAction action)
            {
                this.name = name;
                this.time = time;
                this.breakable = breakable;
                this.action = action;
            }

            public ActionData(float time, bool breakable, IAction action) : this(string.Empty, time, breakable, action)
            {

            }
        }

        public ActionType Type { get; set; }
        public IAction CurrentAction => _currentAction;
        public Animator Animator { get; set; }
        private Queue<IAction> _queue;
        private volatile IAction _currentAction;
        Action()
        {
            _currentAction = Free;
        }
        private void Awake()
        {
            Animator = GetComponent<Animator>();
            _queue = new Queue<IAction>();
        }

        private void FixedUpdate()
        {
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
