using Base2D.System.ActionSystem.DelayAction;
using Base2D.System.ActionSystem.BreakAtion;
using UnityEngine;

namespace Base2D.System.ActionSystem
{
    public abstract class Action : MonoBehaviour
    {
        public ActionType Type { get; set; }
        public string Name { get; set; }
        public Animator Animation { get; set; }
        abstract public bool BreakAction(BreakType breakType);
        abstract public bool DelayAction(DelayInfo delayInfo);
        public virtual void Play(string animation)
        {
            Animation.Play(animation);
        }

        public virtual void PlayQueued(string animation, QueueMode queue, PlayMode play)
        {
            // Animation.PlayQueued(animation, queue, play);
        }
    }
}
