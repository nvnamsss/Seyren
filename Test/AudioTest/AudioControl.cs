using Crom.System.UnitSystem;
using Crom.System.BuffSystem;
using Crom.System.BuffSystem.ScriptableObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Crom.Test.AudioTest
{
    public class AudioControl : MonoBehaviour
    {
        public AudioSource audioSource;

        

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                audioSource.Play();
            }
            if (Input.GetKey(KeyCode.B))
            {
                audioSource.Stop();
            }
        }

    }
}
