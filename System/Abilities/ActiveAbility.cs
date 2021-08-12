using Seyren.System.Generics;
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
    public abstract class ActiveAbility : Ability
    {
        protected enum CooldownType
        {
            WhenCasting,
            WhenCancelled,
            WhenCast,

        }

        // public event GameEventHandler<ActiveAbility, CastingSpellEventArgs> OnCasting;
        // public event GameEventHandler<ActiveAbility> OnCast;
        public float castTime;
        // public float CastTimeRemaining { get; set; }
        // public bool IsCasting { get; set; }
        // public float CastInterval { get; set; }
        // bool cancel;
        bool casting;
        protected CooldownType cooldownType;
        protected abstract void DoWhenCasting();
        protected abstract void DoCastAbility();
        public ActiveAbility(int level) : base(level)
        {
            CastType = CastType.Active;
            casting = false;
            // cancel = false;
        }

        public void Cancel()
        {
            // cancel = true;
            casting = false;
            if (cooldownType == CooldownType.WhenCancelled)
            {
                cooldown();
            }
        }

        public override Error Cast(Unit by)
        {
            abilityTarget = AbilityTarget.NoTarget(by);
            onCast();
            return null;
        }

        public override Error Cast(Unit by, Unit target)
        {
            abilityTarget = AbilityTarget.UnitTarget(by, target);
            onCast();
            return null;
        }
        public override Error Cast(Unit by, Vector3 location)
        {
            abilityTarget = AbilityTarget.PointTarget(by, location);
            onCast();
            return null;
        }

        protected override async void onCast()
        {
            casting = true;
            int c = (int)(castTime * 1000);
            if (cooldownType == CooldownType.WhenCasting)
            {
                cooldown();
            }
            DoWhenCasting();

            if (!casting) return;
            await Task.Delay(c);
            if (!casting) return;

            DoCastAbility();
            if (cooldownType == CooldownType.WhenCast)
            {
                cooldown();
            }
        }

        protected void cooldown()
        {
            nextCooldown = DateTimeOffset.Now.ToUnixTimeMilliseconds() + (long)(Cooldown * 1000);
        }
    }
}
