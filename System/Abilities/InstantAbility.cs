using Seyren.System.Generics;
using Seyren.System.Units;
using System.Collections;
using UnityEngine;

namespace Seyren.System.Abilities
{
    public abstract class InstantAbility : Ability
    {
        public event GameEventHandler<InstantAbility> Casted;

        protected abstract void DoCastAbility();
        public InstantAbility(float cooldown, int level) : base(cooldown, 1)
        {
            CastType = CastType.Instant;
        }
        
        
        // public override bool Cast()
        // {
        //     if (!Condition())
        //     {
        //         return false;
        //     }

        //     Casted?.Invoke(this);
        //     DoCastAbility();
        //     return true;
        // }

    }
}
