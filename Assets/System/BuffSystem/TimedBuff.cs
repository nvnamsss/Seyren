using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crom.System.BuffSystem.ScriptableObject;

namespace Crom.System.BuffSystem
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