using System;
using Seyren.System.Units;
using Seyren.Universe;
using UnityEngine;

namespace Seyren.Projectiles
{
    /// <summary>
    /// A projectile that calculates a trajectory to reach a specific target location.
    /// Can use an arc trajectory for more realistic movement.
    /// </summary>
    public class TargetProjectile : IProjectile
    {
        public GameObject gameObject;
        private Vector3 location;
        private Vector3 startPosition;
        private Vector3 targetPosition;
        public Quaternion rotation;
        public float speed = 10f;
        public float lifeTime = 5f;
        private float initialLifeTime;
        private float elapsedTime = 0f;

        // Arc trajectory parameters
        public float arcRate = 0f;  // 0 = straight line, 1 = full arc
        public float arcHeight = 2f; // Maximum height of the arc

        private string _type = "Target";
        private string _id = Guid.NewGuid().ToString();
        private ObjectStatus _objectStatus = ObjectStatus.None;
        private Vector3 _size = Vector3.one;
        private float totalDistance;
        private float estimatedTime;
        private bool hasReachedTarget = false;

        public string Type { get => _type; set => _type = value; }
        public float Speed { get => speed; set => speed = value; }
        public float LifeTime { get => lifeTime; set => lifeTime = value; }
        public string ID => _id;
        public ObjectStatus ObjectStatus { get => _objectStatus; set => _objectStatus = value; }
        public Vector3 Location => location;
        public Vector3 Size { get => _size; set => _size = value; }
        public Quaternion Rotation => rotation;

        public bool IsActive => isActive;
        private bool isActive = false;

        public Action<TargetProjectile> onTick;
        public Action<TargetProjectile> onTargetReached;

        /// <summary>
        /// Creates a projectile that will travel to a target location.
        /// </summary>
        /// <param name="gameObject">The GameObject representing the projectile</param>
        /// <param name="targetPosition">The destination position</param>
        /// <param name="speed">Movement speed</param>
        /// <param name="lifeTime">Maximum lifetime</param>
        /// <param name="arcRate">Arc intensity (0=straight line, 1=full arc)</param>
        /// <param name="arcHeight">Maximum height of the arc</param>
        public TargetProjectile(GameObject gameObject, Vector3 targetPosition, float speed, float lifeTime, float arcRate = 0f, float arcHeight = 2f)
        {
            this.gameObject = gameObject;
            this.startPosition = gameObject.transform.position;
            this.location = this.startPosition;
            this.targetPosition = targetPosition;
            this.speed = speed;
            this.lifeTime = lifeTime;
            this.initialLifeTime = lifeTime;
            this.arcRate = Mathf.Clamp01(arcRate);
            this.arcHeight = arcHeight;
            
            // Calculate initial rotation to face target
            Vector3 initialDirection = (targetPosition - this.startPosition).normalized;
            this.rotation = Quaternion.LookRotation(initialDirection);
            
            // Set initial rotation of the game object
            gameObject.transform.rotation = this.rotation;
            
            // Calculate total distance and estimated arrival time
            this.totalDistance = Vector3.Distance(this.startPosition, this.targetPosition);
            this.estimatedTime = this.totalDistance / speed;
            
            isActive = true;
        }

        public void Loop(ITime time)
        {
            // Decrease lifetime and check if expired
            lifeTime -= time.DeltaTime;
            if (lifeTime <= 0f)
            {
                if (gameObject != null) UnityEngine.Object.Destroy(gameObject);
                isActive = false;
                return;
            }

            // Increment elapsed time
            elapsedTime += time.DeltaTime;
            
            // Calculate progress ratio (0 to 1)
            float t = Mathf.Clamp01(elapsedTime / estimatedTime);
            
            // Check if we've reached the target
            if (t >= 1.0f && !hasReachedTarget)
            {
                location = targetPosition;
                hasReachedTarget = true;
                onTargetReached?.Invoke(this);
            }
            else if (!hasReachedTarget)
            {
                // Calculate position using linear interpolation for horizontal movement
                Vector3 straightPosition = Vector3.Lerp(startPosition, targetPosition, t);
                
                if (arcRate > 0f)
                {
                    // Add arc height using a sine curve (peaks in the middle of the journey)
                    float arcOffset = Mathf.Sin(t * Mathf.PI) * arcHeight * arcRate;
                    location = straightPosition + Vector3.up * arcOffset;
                    
                    // Calculate new direction for rotation based on path tangent
                    if (time.DeltaTime > 0)
                    {
                        Vector3 prevPos = gameObject.transform.position;
                        Vector3 direction = (location - prevPos).normalized;
                        if (direction != Vector3.zero)
                        {
                            rotation = Quaternion.LookRotation(direction);
                        }
                    }
                }
                else
                {
                    // No arc - straight line movement
                    location = straightPosition;
                }
            }

            // Update GameObject position and rotation
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
        }
        
        /// <summary>
        /// Gets the remaining time until the projectile reaches its target
        /// </summary>
        public float GetRemainingTimeToTarget()
        {
            if (hasReachedTarget)
                return 0;
                
            return Mathf.Max(0, estimatedTime - elapsedTime);
        }
        
        /// <summary>
        /// Gets the progress toward target as a value between 0 and 1
        /// </summary>
        public float GetProgressToTarget()
        {
            return Mathf.Clamp01(elapsedTime / estimatedTime);
        }
    }
}
