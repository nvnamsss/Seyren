using System;
using Seyren.System.Units;
using Seyren.Universe;
using UnityEngine;

namespace Seyren.Projectiles
{
    public class InstantProjectile : IProjectile
    {
        public string Type { get => _type; }
        public float Speed { get => _speed; set => _speed = value; }
        public float LifeTime { get => _lifeTime; set => _lifeTime = value; }

        public string ID => _id;
        private string _id = Guid.NewGuid().ToString();

        public ObjectStatus ObjectStatus { get => _objectStatus; set => _objectStatus = value; }

        public bool IsActive { get; private set; }

        public Vector3 Location => _location;

        public Vector3 Size { get => _size; set => _size = value; }

        public Quaternion Rotation => _rotation;

        public Vector3 Forward => throw new NotImplementedException();

        private string _type = "Instant";
        private float _speed = 0f; // Instant hit, speed is for interface compatibility
        private float _lifeTime = 0.1f; // Very short lifetime as hitscan is instant
        private ObjectStatus _objectStatus = ObjectStatus.None;
        private Vector3 _location;
        private Vector3 _size = Vector3.one;
        private Quaternion _rotation;

        public event Action<IProjectile> OnCompleted;
        public GameObject gameObject;

        public InstantProjectile(GameObject gameObject)
        {
            this.gameObject = gameObject;
            IsActive = true;
        }

        public void Loop(ITime time)
        {
            if (!IsActive)
            {
                return; // If not active, do nothing
            }

            // hit instantly
            Revoke();
        }

        public void Revoke()
        {
            IsActive = false;
            OnCompleted?.Invoke(this);
        }
    }
}