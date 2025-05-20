using Seyren.System.Units;
using Seyren.Universe;
using UnityEngine;
using System;

namespace Seyren.Projectiles
{
    public class BezierProjectile : IProjectile
    {
        private string _type;
        private float _speed;
        private float _lifeTime;
        private string _id;
        private ObjectStatus _objectStatus;
        private bool _isActive;
        private Vector3 _location;
        private Vector3 _size;
        private Quaternion _rotation;
        
        // Bezier curve control points
        private Vector3[] _controlPoints;
        private float _currentTime;
        private float _totalTime;
        private GameObject _projectileObject;
        private Action<BezierProjectile> _onComplete;

        public string Type { get => _type; set => _type = value; }
        public float Speed { get => _speed; set => _speed = value; }
        public float LifeTime { get => _lifeTime; set => _lifeTime = value; }
        public string ID => _id;
        public ObjectStatus ObjectStatus { get => _objectStatus; set => _objectStatus = value; }
        public bool IsActive => _isActive;
        public Vector3 Location => _location;
        public Vector3 Size { get => _size; set => _size = value; }
        public Quaternion Rotation => _rotation;
        public Action<BezierProjectile> onTick;

        /// <summary>
        /// Creates a new Bezier projectile
        /// </summary>
        /// <param name="id">Unique identifier</param>
        /// <param name="type">Projectile type</param>
        /// <param name="startPoint">Start position</param>
        /// <param name="controlPoint1">First control point</param>
        /// <param name="controlPoint2">Second control point</param>
        /// <param name="endPoint">End position</param>
        /// <param name="speed">Movement speed</param>
        /// <param name="lifeTime">Maximum lifetime</param>
        /// <param name="onComplete">Action to call when projectile completes its path</param>
        public BezierProjectile(GameObject gameObject, Vector3 startPoint, Vector3 controlPoint1, 
                               Vector3 controlPoint2, Vector3 endPoint, float speed, float lifeTime,
                               Action<BezierProjectile> onComplete = null)
        {
            _projectileObject = gameObject;
            _speed = speed;
            _lifeTime = lifeTime;
            _isActive = true;
            _rotation = Quaternion.identity;
            _size = Vector3.one;
            
            // Initialize Bezier curve
            _controlPoints = new Vector3[] { startPoint, controlPoint1, controlPoint2, endPoint };
            _location = startPoint;
            _currentTime = 0f;
            
            // Calculate total time based on path length and speed
            _totalTime = EstimatePathLength() / speed;
            _onComplete = onComplete;
        }


        public void Loop(ITime time)
        {
            if (!_isActive) return;

            // Update lifetime
            _lifeTime -= time.DeltaTime;
            if (_lifeTime <= 0)
            {
                Revoke();
                return;
            }

            // Update position along the Bezier curve
            _currentTime += time.DeltaTime;
            float t = Mathf.Clamp01(_currentTime / _totalTime);

            _location = CalculateBezierPoint(t, _controlPoints[0], _controlPoints[1], _controlPoints[2], _controlPoints[3]);

            // Update the direction the projectile is facing
            if (t < 0.99f)
            {
                Vector3 nextPos = CalculateBezierPoint(t + 0.01f, _controlPoints[0], _controlPoints[1], _controlPoints[2], _controlPoints[3]);
                Vector3 direction = (nextPos - _location).normalized;
                if (direction != Vector3.zero)
                {
                    _rotation = Quaternion.LookRotation(direction);
                }
            }

            // Update visual representation
            if (_projectileObject != null)
            {
                _projectileObject.transform.position = _location;
                _projectileObject.transform.rotation = _rotation;
            }

            // Check if we've reached the end of the path
            if (t >= 1.0f)
            {
                _onComplete?.Invoke(this);
                Revoke();
            }

            onTick?.Invoke(this);
            
        }

        public void Revoke()
        {
            _isActive = false;
            
            // Clean up the visual representation
            if (_projectileObject != null)
            {
                UnityEngine.Object.Destroy(_projectileObject);
            }
        }
        
        /// <summary>
        /// Calculates a point along a cubic Bezier curve
        /// </summary>
        private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;
            
            // Formula: (1-t)³P₀ + 3(1-t)²tP₁ + 3(1-t)t²P₂ + t³P₃
            Vector3 point = uuu * p0;
            point += 3 * uu * t * p1;
            point += 3 * u * tt * p2;
            point += ttt * p3;
            
            return point;
        }
        
        /// <summary>
        /// Estimates the total length of the Bezier curve by sampling multiple points
        /// </summary>
        private float EstimatePathLength(int segments = 20)
        {
            float length = 0;
            Vector3 prevPoint = _controlPoints[0];
            
            for (int i = 1; i <= segments; i++)
            {
                float t = i / (float)segments;
                Vector3 currentPoint = CalculateBezierPoint(t, _controlPoints[0], _controlPoints[1], _controlPoints[2], _controlPoints[3]);
                length += Vector3.Distance(prevPoint, currentPoint);
                prevPoint = currentPoint;
            }
            
            return length;
        }
        
        /// <summary>
        /// Updates the target endpoint of the projectile
        /// </summary>
        public void UpdateTargetPosition(Vector3 newTarget)
        {
            // Keep the same start and control points, but update the endpoint
            _controlPoints[3] = newTarget;
            
            // Recalculate total time based on new path length
            _totalTime = EstimatePathLength() / _speed;
        }
    }
}