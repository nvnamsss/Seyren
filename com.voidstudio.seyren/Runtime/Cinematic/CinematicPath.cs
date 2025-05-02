using UnityEngine;
using System;

namespace Seyren.Cinematic
{
    public class CinematicPath : MonoBehaviour
    {
        public Transform[] waypoints;
        public float duration = 5f;
        public bool playOnStart = true;
        private float timer = 0f;
        private int current = 0;
        private Camera cam;
        private bool playing = false;

        void Start()
        {
            cam = GetComponent<Camera>();
            // Automatically find waypoints by tag if not assigned
            if ((waypoints == null || waypoints.Length == 0))
            {
                var found = GameObject.FindGameObjectsWithTag("Waypoint");
                Array.Sort(found, (a, b) => string.CompareOrdinal(a.name, b.name));
                waypoints = new Transform[found.Length];
                for (int i = 0; i < found.Length; ++i)
                    waypoints[i] = found[i].transform;
            }
            if (playOnStart && waypoints != null && waypoints.Length > 1)
                Play();
        }

        public void Play()
        {
            timer = 0f;
            current = 0;
            playing = true;
        }

        void Update()
        {
            if (!playing || waypoints == null || waypoints.Length < 2) return;

            timer += Time.deltaTime;
            float t = timer / duration;
            if (t >= 1f)
            {
                cam.transform.position = waypoints[waypoints.Length - 1].position;
                cam.transform.rotation = waypoints[waypoints.Length - 1].rotation;
                playing = false;
                return;
            }

            // Interpolate along the path
            float pathT = t * (waypoints.Length - 1);
            int idx = Mathf.FloorToInt(pathT);
            float lerpT = pathT - idx;
            if (idx >= waypoints.Length - 1) idx = waypoints.Length - 2;

            cam.transform.position = Vector3.Lerp(waypoints[idx].position, waypoints[idx + 1].position, lerpT);
            cam.transform.rotation = Quaternion.Slerp(waypoints[idx].rotation, waypoints[idx + 1].rotation, lerpT);
        }
    }
}
