using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base2D.System.AbilitySystem
{
    public abstract class ChannelAbility : Ability
    {
        public delegate void ChannelHandler(ChannelAbility sender);
        public event ChannelHandler ChannelStart;
        public event ChannelHandler ChannelEnd;

        public BreakType BreakType { get; set; }
        public bool IsChanneling { get; set; }
        public float Interval { get; set; }
        public float ChannelTime { get; set; }
        public float ChannelTimeLeft { get; set; }
        public float TotalChannelTime;
        public ChannelAbility(Unit caster, float channelTime, float interval, float cooldown, int level) : 
            base(caster, channelTime, cooldown, level)
        {
            CastType = CastType.Channel;
            Interval = interval;
            TotalChannelTime = 0;
        }

        public override bool Cast()
        {
            if (!Condition())
            {
                return false;
            }

            ChannelStart?.Invoke(this);
            Caster.StartCoroutine(Channel(Interval, ChannelTime));
            return true;
        }

        protected abstract override bool Condition();
        protected abstract void DoChannelAbility();
        protected IEnumerator Channel(float timeDelay, float channelTime)
        {
            IsChanneling = true;
            ChannelTimeLeft = channelTime;
            TotalChannelTime = 0;

            while (ChannelTimeLeft >= 0)
            {
                yield return new WaitForSeconds(timeDelay);
                DoChannelAbility();
                ChannelTimeLeft -= timeDelay;
                TotalChannelTime += timeDelay;
            }

            Caster.StartCoroutine(Casted(0, BaseCoolDown));
            yield break;
        }

        protected IEnumerator Casted(float timeDelay, float cooldown)
        {
            ChannelEnd?.Invoke(this);
            IsCastable = false;
            IsChanneling = false;
            TimeCoolDownLeft = cooldown - timeDelay;

            while (TimeCoolDownLeft >= 0)
            {
                yield return new WaitForSeconds(timeDelay);
                TimeCoolDownLeft -= timeDelay;
            }
            IsCastable = true;
        }
    }
}
