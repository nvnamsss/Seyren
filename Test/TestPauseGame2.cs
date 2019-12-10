using Base2D.System.UnitSystem;
using Base2D.System.ActionSystem;
using Base2D.System.ActionSystem.BreakAtion;
using Base2D.System.ActionSystem.DelayAction;
using Base2D.System.BuffSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base2D.Test
{
    public class TestPauseGame2 : MonoBehaviour
    {
        private float baseTime;

        public void Start()
        {
            StartCoroutine(testloop());
            baseTime = Time.timeScale;
        }
        

        IEnumerator testloop()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                Debug.Log("test2");
            }
        }
    }
}
