using UnityEngine;
using System.IO;
using System.Collections;

namespace Seyren.Cinematic
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(CinematicPath))]
    public class CinematicCamera : MonoBehaviour
    {
        public string saveFolder = "CinematicCaptures";
        public int imageWidth = 512;
        public int imageHeight = 512;
        public float captureDelay = 0.1f; // Time to wait at each waypoint before capture

        private Camera cam;
        private CinematicPath path;

        void Start()
        {
            cam = GetComponent<Camera>();
            path = GetComponent<CinematicPath>();
            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);

            StartCoroutine(CaptureAlongPath());
        }

        private IEnumerator CaptureAlongPath()
        {
            if (path.waypoints == null || path.waypoints.Length < 2)
            {
                Debug.LogError("CinematicCamera: Not enough waypoints.");
                yield break;
            }

            for (int i = 0; i < path.waypoints.Length; ++i)
            {
                cam.transform.position = path.waypoints[i].position;
                cam.transform.rotation = path.waypoints[i].rotation;
                yield return new WaitForSeconds(captureDelay);

                RenderTexture rt = new RenderTexture(imageWidth, imageHeight, 24);
                cam.targetTexture = rt;
                Texture2D screenShot = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);
                cam.Render();
                RenderTexture.active = rt;
                screenShot.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
                screenShot.Apply();
                cam.targetTexture = null;
                RenderTexture.active = null;
                DestroyImmediate(rt);

                string filename = Path.Combine(saveFolder, $"cinematic_{i:D2}.png");
                File.WriteAllBytes(filename, screenShot.EncodeToPNG());
            }
            Debug.Log($"CinematicCamera: Captured {path.waypoints.Length} images to {saveFolder}");
        }
    }
}
