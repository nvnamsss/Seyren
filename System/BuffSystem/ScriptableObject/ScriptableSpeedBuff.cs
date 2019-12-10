using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crom.System.BuffSystem.ScriptableObject
{
    public class SpeedBuff : ScriptableBuff
    {
        public float SpeedIncrease;

        public override TimedBuff InitializeBuff(GameObject obj)
        {
            return new TimedSpeedBuff(Duration, this, obj);
        }
    }
}