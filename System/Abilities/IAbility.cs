using Seyren.System.Units;
using Seyren.System.Actions;
using Seyren.System.Buffs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.System.Abilities
{
    public interface IAbility
    {
        CastType AbilityType { get; set; }

        AudioClip Sound { get; set; }
        GameObject BaseUnit { get; set; }
        Animation BaseAnimation { get; set; }

        float BaseCoolDown { get; set; }
        float TimeCoolDownLeft { get; set; }
        float BaseCastingTime { get; set; }
        float TimeCastingLeft { get; set; }

        float TimeUpdate { get; set; }
        
        bool IsCastable { get; set; }
        bool IsCasting { get; set; }

        GameObject ObjectTarget { get; set; }
        Vector3 PointTarget { get; set; }

        void UnlockAbility();
        bool TryCastAbility(GameObject target);
        bool TryCastAbility(Vector3 target);


    }
}
