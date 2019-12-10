using System.Collections.Generic;
using UnityEngine;

namespace Crom.System.UnitSystem
{
    public class Action
    {
        public Animation Animation { get; }

        public Action(Animation animation)
        {
            Animation = animation;
        }
        public void Play(string animation)
        {
            Animation.Play(animation);
        }

        public void PlayQueued(string animation, QueueMode queue, PlayMode play)
        {
            Animation.PlayQueued(animation, queue, play);
        }

    }
}
