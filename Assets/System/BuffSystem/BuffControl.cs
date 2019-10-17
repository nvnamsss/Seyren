using Assets.System.BuffSystem;
using Crom.System.UnitSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffControl : MonoBehaviour
{
    public Unit unitGiveEffect { get; set; }
    public Unit unitReceiveEffect { get; set; }
    public BuffType bufftype { get; set; }
    public Time timeCountDown { get; set; }
    public float value { get; set; }

    //Init
    public BuffControl(Unit UnitGiveEffect, Unit UnitReceiveEffect)
    {
        
    }

    //
    public void TimeOut()
    {

    }
}
