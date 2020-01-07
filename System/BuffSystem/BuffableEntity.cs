using System.Collections;
using Base2D.System.ActionSystem;
using System.Collections.Generic;
using UnityEngine;
using Base2D.System.BuffSystem.ScriptableObject;
using Base2D.System.ActionSystem.BreakAtion;
using Base2D.System.ActionSystem.DelayAction;

namespace Base2D.System.BuffSystem
{
    public class BuffableEntity : Action
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

        protected override void Tick(float time)
        {
            throw new global::System.NotImplementedException();
        }
    }
}