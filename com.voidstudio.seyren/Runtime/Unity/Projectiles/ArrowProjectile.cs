using Seyren.System.Units;
using Seyren.System.Units.Projectiles;
using UnityEngine;

namespace Seyren.System.Universe.Projectiles
{
    public class ArrowProjectile : MonoBehaviour, IObject
    {
        public GameObject source;
        public float cooldonw = 1.0f;
        private float lastFireTime = 0;
        

        public ObjectStatus ObjectStatus { get => throw new global::System.NotImplementedException(); set => throw new global::System.NotImplementedException(); }

        public Vector3 Location => throw new global::System.NotImplementedException();

        public Vector3 Size { get => throw new global::System.NotImplementedException(); set => throw new global::System.NotImplementedException(); }

        public Quaternion Rotation => throw new global::System.NotImplementedException();

        void Start()
        {

        }

        void Update()
        {
            // transform.position = projectile.G
            if (Input.GetMouseButtonDown(0) && lastFireTime + cooldonw < Time.time)
            {
                lastFireTime = Time.time;
            }
        }
    }
}