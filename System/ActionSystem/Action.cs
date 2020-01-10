using Base2D.System.ActionSystem.DelayAction;
using Base2D.System.ActionSystem.BreakAtion;
using UnityEngine;

namespace Base2D.System.ActionSystem
{
    [DisallowMultipleComponent]
    public class Action : MonoBehaviour
    {
        public delegate void ActionChangeHandler(Action sender, ActionEventArgs e);
        public event ActionChangeHandler ActionChanging;
        public event ActionChangeHandler ActionChanged;
        public ActionType Type
        {
            get
            {
                return _type;
            }
            set
            {
                ActionEventArgs acing = new ActionEventArgs(_type, value);
                ActionChanging?.Invoke(this, acing);
                _type = acing.New;
                
                if (!acing.Changed)
                {
                    ActionChanged?.Invoke(this, acing);
                }
            }
        }
        public string Name { get; set; }
        public Animator Animator { get; set; }
        private ActionType _type;
        public virtual bool BreakAction(BreakType breakType)
        {
            return false;
        }
        public virtual bool DelayAction(DelayInfo delayInfo)
        {
            return false;

        }
        public virtual void Play(string animation)
        {
            Animator.Play(animation);
        }

        public virtual void PlayQueued(string animation, QueueMode queue, PlayMode play)
        {
            // Animation.PlayQueued(animation, queue, play);
        }

        void Awake()
        {
            Animator = GetComponent<Animator>();
        }
        void Start()
        {
        }

        void FixedUpdate()
        {
            Tick(Time.deltaTime);
        }

        protected virtual void Tick(float time)
        {

        }
    }
}
