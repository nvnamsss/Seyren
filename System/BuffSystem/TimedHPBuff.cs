using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base2D.System.BuffSystem.ScriptableObject;
using Base2D.System.UnitSystem;
using Base2D.System.UnitSystem.Units;

namespace Base2D.System.BuffSystem
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
            unit.Attribute.MaxHp += hpBuff.HPIncrease;
        }

        public override void End()
        {
            unit.Attribute.MaxHp -= hpBuff.HPIncrease;
        }
    }
}