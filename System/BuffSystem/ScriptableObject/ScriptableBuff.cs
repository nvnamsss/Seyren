﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base2D.System.BuffSystem.ScriptableObject
{
    public abstract class ScriptableBuff : MonoBehaviour
    {

        //Duration of the buff
        public float Duration { get; set; }

        public abstract TimedBuff InitializeBuff(GameObject obj);

    }
}
