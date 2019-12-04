using System.Collections;
using Crom.System.ActionSystem;
using System.Collections.Generic;
using UnityEngine;
using Crom.System.BuffSystem.ScriptableObject;
using Crom.System.ActionSystem.BreakAtion;
using Crom.System.ActionSystem.DelayAction;

namespace Crom.System.BuffSystem
{
    public class BuffableEntity : IAction
    {

        //List of all current buffs
        public List<TimedBuff> CurrentBuffs = new List<TimedBuff>();
        public float UpdateTime { get; set; }

        void Update()
        {
            //OPTIONAL, return before updating each buff if game is paused
            //if (Game.isPaused)
            //    return;

            foreach (TimedBuff buff in CurrentBuffs.ToArray())
            {
                buff.Tick(UpdateTime);
                if (buff.IsFinished)
                {
                    CurrentBuffs.Remove(buff);
                }
            }
        }

        public void AddBuff(TimedBuff buff)
        {
            CurrentBuffs.Add(buff);
            buff.Activate();
        }

        public override bool BreakAction(BreakType breakType)
        {
            throw new global::System.NotImplementedException();
        }

        public override bool DelayAction(DelayInfo delayInfo)
        {
            throw new global::System.NotImplementedException();
        }
    }
}