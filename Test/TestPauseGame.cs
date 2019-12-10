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
