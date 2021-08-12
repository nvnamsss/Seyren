using Seyren.System.Generics;
using Seyren.System.Units;
using System;
using System.Collections;
using System.Threading.Tasks;
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
        private long lastChannelTime;
        public ChannelAbility(int level) :
            base(level)
        {
            CastType = CastType.Channel;
            TotalChannelTime = 0;
        }


        public void Cancel()
        {
            IsChanneling = false;
        }

        protected override void onCast()
        {
            ChannelAsync(ChannelInterval, ChannelTime);
        }

        protected abstract void DoChannelAbility();
        protected async void ChannelAsync(float interval, float channelTime)
        {
            int linterval = (int)(interval * 1000);
            IsChanneling = true;
            ChannelStart?.Invoke(this);

            while (IsChanneling && ChannelTimeRemaining >= 0)
            {
                DoChannelAbility();
                await Task.Delay(linterval);
                ChannelTimeRemaining -= interval;
                TotalChannelTime += interval;
            }

            ChannelEnd?.Invoke(this);
            IsChanneling = false;
        }


        protected IEnumerator Channel(float interval, float channelTime)
        {
            IsChanneling = true;
            ChannelTimeRemaining = channelTime;
            TotalChannelTime = 0;
            ChannelStart?.Invoke(this);

            while (IsChanneling && ChannelTimeRemaining >= 0)
            {
                yield return new WaitForSeconds(interval);
                DoChannelAbility();
                ChannelTimeRemaining -= interval;
                TotalChannelTime += interval;
            }

            ChannelEnd?.Invoke(this);
            IsChanneling = false;
            yield break;
        }

    }
}
