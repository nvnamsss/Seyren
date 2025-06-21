using System;
using Seyren.System.Units;
using Seyren.Universe;
using UnityEngine;

namespace Seyren.Projectiles
{
    public class CommonProjectile : IProjectile
    {
        public GameObject gameObject;
        private Vector3 location;
        public Quaternion rotation;
        public Vector3 direction;
        public float speed = 10f;
        public float lifeTime = 5f;

        // Arc trajectory properties
        private Vector3 initialPosition;
        private float distanceTraveled = 0f;
        public float arcRate = 0f;  // 0 = straight line, 1 = full arc
        public float arcHeight = 2f; // Maximum height of the arc

        private string _type = "Common";
        private string _id = Guid.NewGuid().ToString();
        private ObjectStatus _objectStatus = ObjectStatus.None;
        private Vector3 _size = Vector3.one;

        public string Type { get => _type; set => _type = value; }
        public float Speed { get => speed; set => speed = value; }
        public float LifeTime { get => lifeTime; set => lifeTime = value; }
        public string ID => _id;
        public ObjectStatus ObjectStatus { get => _objectStatus; set => _objectStatus = value; }
        public Vector3 Location => location;
        public Vector3 Size { get => _size; set => _size = value; }
        public Quaternion Rotation => rotation;

        public bool IsActive => isActive;

        public Vector3 Forward => throw new NotImplementedException();

        private bool isActive = false;

        public Action<CommonProjectile> onTick;

        public event Action<IProjectile> OnCompleted;

        public CommonProjectile(GameObject gameObject, Vector3 direction, float speed, float lifeTime)
        {
            this.gameObject = gameObject;
            this.location = gameObject.transform.position;
            this.initialPosition = this.location;
            this.direction = direction.normalized;
            this.speed = speed;
            this.lifeTime = lifeTime;
            this.rotation = Quaternion.LookRotation(this.direction);
            gameObject.transform.rotation = rotation;
            isActive = true;
        }

        // Constructor overload with arc parameters
        public CommonProjectile(GameObject gameObject, Vector3 direction, float speed, float lifeTime, float arcRate, float arcHeight = 2f)
            : this(gameObject, direction, speed, lifeTime)
        {
            this.arcRate = Mathf.Clamp01(arcRate);
            this.arcHeight = arcHeight;
        }

        public void Loop(ITime time)
        {
            lifeTime -= time.DeltaTime;
            if (lifeTime <= 0f)
            {
                if (gameObject != null) UnityEngine.Object.Destroy(gameObject);
                Revoke();
                return;
            }

            // Calculate distance traveled in this frame
            float frameDistance = speed * time.DeltaTime;
            distanceTraveled += frameDistance;

            // Calculate base movement along direction
            Vector3 straightPosition = initialPosition + direction * distanceTraveled;
            
            if (arcRate > 0f)
            {
                // Calculate normalized progress (0 to 1) for the sine wave
                float totalDistance = speed * this.LifeTime;
                float normalizedProgress = Mathf.Clamp01(distanceTraveled / totalDistance);
                
                // Use sin curve for a single arc (0 to π maps to 0 to 1 in normalized progress)
                float arcOffset = Mathf.Sin(normalizedProgress * Mathf.PI) * arcHeight * arcRate;
                
                // Apply the arc offset in the up direction
                location = straightPosition + Vector3.up * arcOffset;
                
                // Recalculate rotation to follow the arc path
                if (frameDistance > 0f)
                {
                    Vector3 directionToFace;
                    if (gameObject != null)
                    {
                        directionToFace = (location - gameObject.transform.position).normalized;
                        if (directionToFace != Vector3.zero)
                        {
                            rotation = Quaternion.LookRotation(directionToFace);
                        }
                    }
                }
            }
            else
            {
                // No arc - straight line movement
                location = straightPosition;
            }

            if (gameObject != null)
            {
                gameObject.transform.position = location;
                gameObject.transform.rotation = rotation;
            }
            
            onTick?.Invoke(this);
        }

        public void Revoke()
        {
            isActive = false;
            OnCompleted?.Invoke(this);
        }
    }
}
