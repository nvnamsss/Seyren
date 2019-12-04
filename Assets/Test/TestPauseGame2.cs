using Crom.System.UnitSystem;
using Crom.System.ActionSystem;
using Crom.System.ActionSystem.BreakAtion;
using Crom.System.ActionSystem.DelayAction;
using Crom.System.BuffSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crom.Test
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
