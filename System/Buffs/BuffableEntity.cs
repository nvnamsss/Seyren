using System.Collections;
using Seyren.System.Actions;
using System.Collections.Generic;
using UnityEngine;
using Seyren.System.Buffs.ScriptableObject;
using Seyren.System.Actions.BreakAtion;
using Seyren.System.Actions.DelayAction;

namespace Seyren.System.Buffs
{
    public class BuffableEntity
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
    }
}