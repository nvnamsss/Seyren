using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crom.System.BuffSystem.ScriptableObject;
using Crom.System.UnitSystem;

namespace Crom.System.BuffSystem
{
    class TimedHPBuff : TimedBuff
    {
        private ScriptableHPBuff hpBuff;

        private Unit unit;

    public TimedHPBuff(float duration, ScriptableBuff buff, GameObject obj) : base(duration, buff, obj)
        {
            unit = obj.GetComponent<Unit>();
            hpBuff = (ScriptableHPBuff)buff;
        }

        public override void Activate()
        {
            hpBuff = (ScriptableHPBuff)buff;
            unit.MaxHp += hpBuff.HPIncrease;
        }

        public override void End()
        {
            unit.MaxHp -= hpBuff.HPIncrease;
        }
    }
}