using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.AbilitySystem
{
    public abstract class ChannelAbility : Ability
    {
        public delegate void ChannellingIntervalHandler(ChannelAbility sender);
        public ChannellingIntervalHandler Channelling;
        public ChannelAbility(Unit caster, float channelTime, float cooldown, int level) : 
            base(caster, channelTime, cooldown, level)
        {
            CastType = CastType.Channel;
        }

        protected abstract override bool Condition();
        protected abstract override void DoCastAbility();
        protected override IEnumerator Casting(float timeDelay, float timeCasting)
        {
            DoCastAbility();
            Caster.StartCoroutine(Casted(0, BaseCoolDown));
            yield break;
        }
    }
}
