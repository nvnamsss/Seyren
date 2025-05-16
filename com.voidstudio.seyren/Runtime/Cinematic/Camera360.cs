using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json; // Add this

namespace Seyren.Cinematic
{
    [Serializable]
    public class WSLPath
    {
        public string distro;
        public string path;

        public string GetPath()
        {
            string normalizedPath = path.Replace('/', '\\');
            normalizedPath = normalizedPath.TrimStart('\\');
            return $@"\\wsl$\{distro}\{normalizedPath}";
        }
    }

    public class Camera360 : MonoBehaviour
    {
        public Transform target;
        public float distance = 5f;
        public int captureCount = 36;
        public string saveFolder = "Camera360Captures";
        public string filenameFormat = "frame_{i:D5}.png";
        
        public WSLPath wslPath;
        public int imageWidth = 512;
        public int imageHeight = 512;
        public string transformsJsonPath = "transforms.json";
        public float fl_x = 0;
        public float fl_y = 0;
        public float cx = 0;
        public float cy = 0;
        public float k1 = 0;
        public float k2 = 0;
        public float p1 = 0;
        public float p2 = 0;
        public string camera_model = "OPENCV";
        
        // Camera noise feature
        public bool enablePositionNoise = false;
        public float noiseAmount = 0.1f;
        public float noiseRange = 2;

        [Serializable]
        public class Frame
        {
            public string file_path;
            public List<List<double>> transform_matrix;
            public int colmap_im_id;
        }

        [Serializable]
        public class Transforms
        {
            public int w;
            public int h;
            public float fl_x;
            public float fl_y;
            public float cx;
            public float cy;
            public float k1;
            public float k2;
            public float p1;
            public float p2;
            public string camera_model;
            public Frame[] frames;
            public List<List<double>> applied_transform;
        }

        private Camera cam;

        void Start()
        {
            cam = GetComponent<Camera>();
            if (cam == null)
            {
                Debug.LogError("Camera360 must be attached to a Camera.");
                enabled = false;
                return;
            }
            if (target == null)
            {
                Debug.LogError("Camera360: Target not set.");
                enabled = false;
                return;
            }
            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);
                
            // Initialize random generator with seed for consistent noise

            Capture360();
        }   

        public void Capture360()
        {
            float angleStep = 360f / captureCount;
            Vector3 offset = new Vector3(0, 0, -distance);

            var frames = new Frame[captureCount];

            for (int i = 0; i < captureCount; i++)
            {
                float angle = i * angleStep;
                Quaternion rot = Quaternion.Euler(0, angle, 0);
                Vector3 pos = target.position + rot * offset;
                
                // Apply position noise if enabled
                if (enablePositionNoise)
                {
                    // Create a random offset in all directions
                    Vector3 noise = new Vector3(
                        UnityEngine.Random.Range(-noiseRange, noiseRange) * noiseAmount,
                        UnityEngine.Random.Range(-noiseRange, noiseRange) * noiseAmount,
                        UnityEngine.Random.Range(-noiseRange, noiseRange) * noiseAmount
                    );
                    Debug.Log($"Camera360: Adding noise {noise} to position {pos}");
                    pos += noise;
                }
                
                cam.transform.position = pos;
                cam.transform.LookAt(target);

                // Save image
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

                byte[] bytes = screenShot.EncodeToPNG();
                string filename = String.Format(filenameFormat, i);
                string filepath = Path.Combine(saveFolder, "images", filename);
                // create directory if it doesn't exist
                string directory = Path.GetDirectoryName(filepath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                    
                // string wslPathString = wslPath.GetPath();
                File.WriteAllBytes(filepath, bytes);
                // File.WriteAllBytes(wslPathString, bytes);

                // --- Extrinsics (camera-to-world) ---
                Matrix4x4 m = cam.transform.localToWorldMatrix;
                var matrix = new List<List<double>>();
                for (int r = 0; r < 4; ++r)
                {
                    var row = new List<double>();
                    for (int c = 0; c < 4; ++c)
                        row.Add(m[r, c]); // Unity is column-major, JSON expects row-major
                    matrix.Add(row);
                }

                frames[i] = new Frame
                {
                    file_path = Path.Combine("images", filename).Replace("\\", "/"),
                    transform_matrix = matrix,
                    colmap_im_id = i
                };
            }

            // --- Intrinsics ---
            float fx = fl_x > 0 ? fl_x : cam.focalLength;
            float fy = fl_y > 0 ? fl_y : cam.focalLength;
            float px = cx > 0 ? cx : imageWidth / 2f;
            float py = cy > 0 ? cy : imageHeight / 2f;

            // --- Applied transform (identity) ---
            var appliedTransform = new List<List<double>>
            {
                new List<double> { 0, 1, 0, 0 },
                new List<double> { 1, 0, 0, 0 },
                new List<double> { 0, 0, -1, 0 }
            };

            var transforms = new Transforms
            {
                w = imageWidth,
                h = imageHeight,
                fl_x = fx,
                fl_y = fy,
                cx = px,
                cy = py,
                k1 = k1,
                k2 = k2,
                p1 = p1,
                p2 = p2,
                camera_model = camera_model,
                frames = frames,
                applied_transform = appliedTransform
            };

            // Use Newtonsoft.Json for serialization
            string json = JsonConvert.SerializeObject(transforms, Formatting.Indented);
            File.WriteAllText(Path.Combine(saveFolder, transformsJsonPath), json);

            Debug.Log($"Camera360: Captured {captureCount} images to {saveFolder} and wrote transforms.json");
        }
    }

}
