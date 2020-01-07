using Base2D.System.ActionSystem.DelayAction;
using Base2D.System.ActionSystem.BreakAtion;
using UnityEngine;

namespace Base2D.System.ActionSystem
{
    [DisallowMultipleComponent]
    public class Action : MonoBehaviour
    {
        public ActionType Type { get; set; }
        public string Name { get; set; }
        public Animator Animator { get; set; }
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
