using Seyren.System.Abilities;
using Seyren.System.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Seyren.System.Abilities
{
    public abstract class ChannelAbility : Ability
    {
        public delegate void ChannelHandler(ChannelAbility sender);
        public event ChannelHandler ChannelStart;
        public event ChannelHandler ChannelEnd;

        public BreakType BreakType { get; set; }
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
        protected Coroutine cooldownCoroutine;
        public ChannelAbility(Unit caster, float channelTime, float interval, float cooldown, int level) : 
            base(caster, cooldown, level)
        {
            CastType = CastType.Channel;
            ChannelInterval = interval;
            ChannelTime = channelTime;
            TotalChannelTime = 0;
        }

        public override bool Cast()
        {
            if (!Condition())
            {
                return false;
            }

            ChannelStart?.Invoke(this);
            channelCoroutine = Caster.StartCoroutine(Channel(ChannelInterval, ChannelTime));
            return true;
        }

        protected abstract override bool Condition();
        protected abstract void DoChannelAbility();
        protected IEnumerator Channel(float interval, float channelTime)
        {
            IsChanneling = true;
            ChannelTimeRemaining = channelTime;
            TotalChannelTime = 0;

            while (ChannelTimeRemaining >= 0)
            {
                yield return new WaitForSeconds(interval);
                DoChannelAbility();
                ChannelTimeRemaining -= interval;
                TotalChannelTime += interval;
            }

            cooldownCoroutine = Caster.StartCoroutine(Casted(CooldownInterval, BaseCoolDown));
            yield break;
        }

        protected IEnumerator Casted(float timeDelay, float cooldown)
        {
            ChannelEnd?.Invoke(this);
            Active = false;
            IsChanneling = false;
            CooldownRemaining = cooldown - timeDelay;

            while (CooldownRemaining >= 0)
            {
                yield return new WaitForSeconds(timeDelay);
                CooldownRemaining -= timeDelay;
            }
            Active = true;
        }
    }
}
