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
            ////Getting MovementComponent, replace with your own implementation
            //movementComponent = obj.GetComponent<MovementComponent>();
            //speedBuff = (ScriptableSpeedBuff)buff;

            unit = obj.GetComponent<Unit>();
            hpBuff = (ScriptableHPBuff)buff;
        }

        public override void Activate()
        {
            ////Add speed increase to MovementComponent
            //ScriptableSpeedBuff speedBuff = (SpeedBuff)buff;
            //movementComponent.moveSpeed += speedBuff.SpeedIncrease;

            hpBuff = (ScriptableHPBuff)buff;
            unit.MaxHp += hpBuff.HPIncrease;
        }

        public override void End()
        {
            ////Revert speed increase
            //movementComponent.moveSpeed -= speedBuff.SpeedIncrease;
            unit.MaxHp += hpBuff.HPIncrease;
        }
    }
}