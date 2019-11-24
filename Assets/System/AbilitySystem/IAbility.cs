using Crom.System.UnitSystem;
using Crom.System.ActionSystem;
using Crom.System.BuffSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crom.System.AbilitySystem
{
    interface IAbility
    {
        AudioClip Sound { get; set; }
        GameObject BaseUnit { get; set; }
        Animation BaseAnimation { get; set; }
        float BaseCoolDown { get; set; }
        float BaseCastingTime { get; set; }
        float TimeCastingLeft { get; set; }
        //Default player cannt case any skill until he unlocked it!
        bool IsCastable { get; set; }
        bool IsCasting { get; set; }

        GameObject ObjectTarget { get; set; }
        GameObject PointTarget { get; set; }

        void UnlockAbility();
        void TrycastAbility(GameObject objectTarget, GameObject pointTarget);


    }
}
