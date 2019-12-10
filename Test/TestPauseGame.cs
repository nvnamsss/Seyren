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
    public class TestPauseGame : MonoBehaviour
    {
        private float baseTime;

        public void Start()
        {
            StartCoroutine(testloop());
            baseTime = Time.timeScale;
        }

        public void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                Time.timeScale = 0;
                Debug.Log("A");
            }
            if (Input.GetKey(KeyCode.B))
            {
                Time.timeScale = baseTime;
                Debug.Log("B");
            }
        }

        IEnumerator testloop()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                Debug.Log("test");
            }
        }
    }
}
