using UnityEngine;

namespace Seyren.Utils
{
    public static class RotationUtils
    {
        /// <summary>
        /// Calculate rotation vector between 2 points in 3d space
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static Vector3 AngleBetween(Vector3 from, Vector3 to)
        {
            Vector3 rotation = new Vector3();
            rotation.x = AngleBetween(from.y, to.y, from.z, to.z);
            rotation.y = AngleBetween(from.x, to.x, from.z, to.z);
            rotation.z = AngleBetween(from.x, to.x, from.y, to.y);
            return rotation;
        }

        public static Quaternion EulerAngleBetween(Vector3 from, Vector3 to)
        {
            Vector3 rotation = AngleBetween(from, to);

            return Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        }

        public static Vector3 GetDirection(Quaternion quaternion)
        {
            return quaternion * Vector3.forward;
        }
        /// <summary>
        /// Calculate angle between 2 point a and b in 2d space <br></br>
        /// example in xy, forward of a will be x and up of a will be y
        /// </summary>
        /// <param name="forward_a">forward of a</param>
        /// <param name="forward_b">forward of b</param>
        /// <param name="up_a">up of a</param>
        /// <param name="up_b">up of b</param>
        /// <returns></returns>
        public static float AngleBetween(float forward_a, float forward_b, float up_a, float up_b)
        {
            return Mathf.Atan2(forward_a - forward_b, up_a - up_b) * Mathf.Rad2Deg;
        }

        public static float AngleBetween(Vector2 from, Vector2 to)
        {
            return AngleBetween(from.x, to.x, from.y, to.y);
        }
    }
}
