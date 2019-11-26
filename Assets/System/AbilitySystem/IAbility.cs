using Crom.System.UnitSystem;
using Crom.System.ActionSystem;
using Crom.System.BuffSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crom.System.AbilitySystem
{
    public interface IAbility
    {
        AbilityType AbilityType { get; set; }

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
        GameObject PointTarget { get; set; }

        void UnlockAbility();
        bool TryCastAbility(GameObject objectTarget, GameObject pointTarget);


    }
}
