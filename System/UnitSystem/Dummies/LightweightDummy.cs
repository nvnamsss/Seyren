using UnityEngine;

namespace Base2D.System.UnitSystem.Dummies
{
    public partial class LightweightDummy : MonoBehaviour
    {
        public bool Active
        {
            get => _active;
            set => _active = value;
        }
        public Vector2 Forward
        {
            get => _forward;
            set => _forward = value;
        }
        [SerializeField]
        protected Vector2 _forward;
        [SerializeField]
        protected bool _active;

        public void Look(Vector2 direction)
        {
            Look(Forward, direction);
        }

        public void Look(Vector2 forward, Vector2 direction)
        {
            if (!Active)
            {
                return;
            }

            float forwardDot = Vector2.Dot(forward, direction);
            Vector2 f = forward * forwardDot;
            Quaternion q1 = Quaternion.FromToRotation(forward, f);
            Quaternion q2 = Quaternion.FromToRotation(f, direction);
            transform.rotation = q2 * q1;
            
        }
    }
}
