using Seyren.System.Generics;
using Seyren.System.Units;
using System;
using System.Collections;
using UnityEngine;

namespace Seyren.System.Abilities
{
    public abstract class ChannelAbility : Ability
    {
        public event GameEventHandler<ChannelAbility> ChannelStart;
        public event GameEventHandler<ChannelAbility> ChannelEnd;
        public bool IsChanneling { get; set; }
        /// <summary>
        /// time between every Channel process  <br></br>
        /// This Interval is using for Channel method
        /// </summary>
        public float ChannelInterval { get; set; }
        public float ChannelTime { get; set; }
        public float ChannelTimeRemaining { get; set; }
        public float TotalChannelTime;
        protected Coroutine channelCoroutine;
        public ChannelAbility(float channelTime, float interval, float cooldown, int level) : 
            base(cooldown, level)
        {
            CastType = CastType.Channel;
            ChannelInterval = interval;
            ChannelTime = channelTime;
            TotalChannelTime = 0;
        }


        // public override bool Cast()
        // {
        //     if (!Condition())
        //     {
        //         return false;
        //     }

        //     ChannelStart?.Invoke(this);
        //     // channelCoroutine = Caster.StartCoroutine(Channel(ChannelInterval, ChannelTime));
        //     return true;
        // }
        protected override void onCast(Unit by)
        {
            throw new NotImplementedException();
        }

        protected override void onCast(Unit by, Unit target)
        {
            throw new NotImplementedException();
        }

        protected override void onCast(Unit by, Vector3 target)
        {
            throw new NotImplementedException();
        }
        
        protected abstract void DoChannelAbility();
        protected IEnumerator Channel(float interval, float channelTime)
        {
            IsChanneling = true;
            ChannelTimeRemaining = channelTime;
            TotalChannelTime = 0;
            ChannelStart?.Invoke(this);

            while (ChannelTimeRemaining >= 0)
            {
                yield return new WaitForSeconds(interval);
                DoChannelAbility();
                ChannelTimeRemaining -= interval;
                TotalChannelTime += interval;
            }

            ChannelEnd?.Invoke(this);
            yield break;
        }

    }
}
