using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Seyren.Unity
{
    [ExecuteInEditMode]
    public class Patrol : MonoBehaviour
    {
        public List<Vector3> waypoints; // Array of waypoints to patrol
        public float speed = 2f; // Speed of the patrol
        private int currentWaypointIndex = 0; // Index of the current waypoint
#if UNITY_EDITOR
        private bool isRecording = false; // Flag to track if we're in recording mode

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            Event e = Event.current;
            
            // Draw the record button
            Handles.BeginGUI();
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.textColor = isRecording ? Color.red : Color.white;
            buttonStyle.fontStyle = isRecording ? FontStyle.Bold : FontStyle.Normal;
            
            if (GUI.Button(new Rect(10, 10, 80, 30), isRecording ? "Stop" : "Record", buttonStyle))
            {
                isRecording = !isRecording;
                e.Use();
            }
            
            if (isRecording)
            {
                GUI.Label(new Rect(100, 10, 200, 30), "Recording waypoints...");
            }
            Handles.EndGUI();

            // Only add waypoints if recording is active
            if (isRecording && e.type == EventType.MouseDown && e.button == 0 && !e.alt)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Undo.RecordObject(this, "Add Point");
                    waypoints.Add(hit.point);
                    Debug.Log("Added point in EditMode: " + hit.point);
                    e.Use(); // Prevent other tools from handling the click
                }
            }
            
            // Draw lines between waypoints for better visualization
            if (waypoints.Count > 1)
            {
                Handles.color = isRecording ? Color.red : Color.green;
                for (int i = 0; i < waypoints.Count; i++)
                {
                    if (waypoints[i] != null)
                    {
                        int nextIndex = (i + 1) % waypoints.Count;
                        if (waypoints[nextIndex] != null)
                        {
                            Handles.DrawLine(waypoints[i], waypoints[nextIndex]);
                        }
                    }
                }
            }
        }
#endif
        void Update()
        {
            // Only run the patrol movement logic in play mode, not in editor
            if (!Application.isPlaying)
                return;

            if (waypoints.Count == 0) return; // No waypoints to patrol

            // Move towards the current waypoint
            Vector3 targetWaypoint = waypoints[currentWaypointIndex];
            Vector3 direction = (targetWaypoint - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Check if we have reached the current waypoint
            if (Vector3.Distance(transform.position, targetWaypoint) < 0.1f)
            {
                // Move to the next waypoint
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            }
        }
    }
}