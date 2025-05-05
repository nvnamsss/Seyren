using UnityEngine;

namespace Seyren.Visualizer
{
    /// <summary>
    /// Utility class to draw cone gizmos for visualizing area effects like FlameSplitter
    /// </summary>
    public static class ConeGizmos
    {
        /// <summary>
        /// Draw a cone gizmo
        /// </summary>
        /// <param name="position">Origin position of the cone</param>
        /// <param name="direction">Direction the cone is facing</param>
        /// <param name="angle">Angle of the cone in degrees</param>
        /// <param name="length">Length (radius) of the cone</param>
        /// <param name="color">Color of the cone gizmo</param>
        /// <param name="segments">Number of segments to use when drawing the cone (higher = smoother)</param>
        public static void DrawCone(Vector3 position, Vector3 direction, float angle, float length, Color color, int segments = 16)
        {
            // Store previous color and matrix
            Color prevColor = Gizmos.color;
            Matrix4x4 prevMatrix = Gizmos.matrix;
            
            // Set new color
            Gizmos.color = color;
            
            // Calculate rotation to align cone with direction
            Quaternion rotation = Quaternion.LookRotation(direction);
            Gizmos.matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
            
            // Calculate angle in radians
            float angleInRad = angle * Mathf.Deg2Rad;
            float coneRadius = Mathf.Tan(angleInRad / 2) * length;

            // Draw the cone tip to base lines
            Vector3[] basePoints = new Vector3[segments];
            for (int i = 0; i < segments; i++)
            {
                float t = i / (float)segments;
                float radians = t * 2 * Mathf.PI;
                
                // Point on the circle at the end of the cone
                Vector3 basePoint = new Vector3(
                    Mathf.Sin(radians) * coneRadius, 
                    Mathf.Cos(radians) * coneRadius, 
                    length
                );
                
                basePoints[i] = basePoint;
                
                // Draw line from origin to this point
                Gizmos.DrawLine(Vector3.zero, basePoint);
            }
            
            // Draw the base of the cone
            for (int i = 0; i < segments; i++)
            {
                Gizmos.DrawLine(basePoints[i], basePoints[(i + 1) % segments]);
            }
            
            // Reset gizmos color and matrix
            Gizmos.color = prevColor;
            Gizmos.matrix = prevMatrix;
        }
        
        /// <summary>
        /// Draw a filled cone gizmo (with triangles)
        /// </summary>
        /// <param name="position">Origin position of the cone</param>
        /// <param name="direction">Direction the cone is facing</param>
        /// <param name="angle">Angle of the cone in degrees</param>
        /// <param name="length">Length (radius) of the cone</param>
        /// <param name="color">Color of the cone gizmo</param>
        /// <param name="segments">Number of segments to use when drawing the cone (higher = smoother)</param>
        public static void DrawSolidCone(Vector3 position, Vector3 direction, float angle, float length, Color color, int segments = 16)
        {
            // Store previous color and matrix
            Color prevColor = Gizmos.color;
            Matrix4x4 prevMatrix = Gizmos.matrix;
            
            // Set new color
            Gizmos.color = color;
            
            // Calculate rotation to align cone with direction
            Quaternion rotation = Quaternion.LookRotation(direction);
            Gizmos.matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
            
            // Calculate angle in radians
            float angleInRad = angle * Mathf.Deg2Rad;
            float coneRadius = Mathf.Tan(angleInRad / 2) * length;
            
            // Draw the cone base segments
            Vector3[] basePoints = new Vector3[segments];
            for (int i = 0; i < segments; i++)
            {
                float t = i / (float)segments;
                float radians = t * 2 * Mathf.PI;
                
                // Point on the circle at the end of the cone
                basePoints[i] = new Vector3(
                    Mathf.Sin(radians) * coneRadius, 
                    Mathf.Cos(radians) * coneRadius, 
                    length
                );
            }
            
            // Draw triangles from origin to base
            for (int i = 0; i < segments; i++)
            {
                // Draw a triangle for each segment
                Vector3 v1 = Vector3.zero; // Tip of cone
                Vector3 v2 = basePoints[i];
                Vector3 v3 = basePoints[(i + 1) % segments];
                
                Gizmos.DrawLine(v1, v2);
                Gizmos.DrawLine(v2, v3);
                Gizmos.DrawLine(v3, v1);
            }
            
            // Draw the base of the cone
            for (int i = 0; i < segments; i++)
            {
                Gizmos.DrawLine(basePoints[i], basePoints[(i + 1) % segments]);
            }
            
            // Reset gizmos color and matrix
            Gizmos.color = prevColor;
            Gizmos.matrix = prevMatrix;
        }
    }
}
