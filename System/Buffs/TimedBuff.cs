using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seyren.System.Buffs.ScriptableObject;

namespace Seyren.System.Buffs
{
    public abstract class TimedBuff : MonoBehaviour
    {

        protected float duration;
        protected ScriptableBuff buff;
        protected GameObject obj;
        public bool IsFinished
        {
            get { return duration <= 0 ? true : false; }
        }

        public TimedBuff(float duration, ScriptableBuff buff, GameObject obj)
        {
            this.duration = duration;
            this.buff = buff;
            this.obj = obj;
        }

        public void Tick(float delta)
        {
            duration -= delta;
            if (duration <= 0)
                End();
        }

        public abstract void Activate();
        public abstract void End();
    }
}