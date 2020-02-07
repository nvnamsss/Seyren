using Base2D.System.ActionSystem.DelayAction;
using Base2D.System.ActionSystem.BreakAtion;
using UnityEngine;
using System.Collections.Generic;

namespace Base2D.System.ActionSystem
{
    [DisallowMultipleComponent]
    public class Action : MonoBehaviour
    {
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
        public Animator Animator { get; set; }
        public ActionData CurrentAction => _currentAction;
        private Queue<ActionData> _queue;
        private ActionData _currentAction;
        private void Awake()
        {
            Animator = GetComponent<Animator>();
            _queue = new Queue<ActionData>();
        }

        private void FixedUpdate()
        {
            _currentAction.time -= Time.fixedDeltaTime;
            if (_currentAction.time <= 0)
            {
                RemoveAction();
            }
        }

        public void Play(IAction action)
        {
            action.EndAction += (s) =>
            {
                RemoveAction();
            };
            action.Invoke();
        }

        public void PlayQueue(ActionData action)
        {
            _queue.Enqueue(action);
            enabled = true;
        }
        
        public void RemoveAction()
        {
            if (_queue.Count > 0)
            {
                _currentAction = _queue.Dequeue();
            }
            else
            {   
                enabled = false;
            }
        }
    }
}
