using System;
using Seyren.System.Units;
using Seyren.Universe;
using UnityEngine;

namespace Seyren.Projectiles
{
    public class HitScanProjectile : IProjectile
    {
        public GameObject gameObject;
        public GameObject hitEffectPrefab;
        public Vector3 location;
        public Vector3 direction;
        public Quaternion rotation;
        public float maxDistance = 100f;
        public LayerMask hitLayers = Physics.DefaultRaycastLayers;

        private string _type = "HitScan";
        private string _id = Guid.NewGuid().ToString();
        private ObjectStatus _objectStatus = ObjectStatus.None;
        private Vector3 _size = Vector3.one;
        private float _speed = 0f; // Instant hit, speed is for interface compatibility
        private float _lifeTime = 0.1f; // Very short lifetime as hitscan is instant

        public string Type { get => _type; set => _type = value; }
        public float Speed { get => _speed; set => _speed = value; }
        public float LifeTime { get => _lifeTime; set => _lifeTime = value; }
        public string ID => _id;
        public ObjectStatus ObjectStatus { get => _objectStatus; set => _objectStatus = value; }
        public Vector3 Location => location;
        public Vector3 Size { get => _size; set => _size = value; }
        public Vector3 Forward => direction.normalized;
        public Quaternion Rotation => rotation;
        public bool IsActive { get; private set; }

        // Callbacks
        public Action<HitScanProjectile, RaycastHit> onHit;
        public Action<HitScanProjectile> onComplete;

        // Interface events
        public event Action<IProjectile> OnStart;
        public event Action<IProjectile> OnCompleted;

        public HitScanProjectile(GameObject sourceObject, GameObject hitEffect, Vector3 direction, float maxDistance = 100f)
        {
            this.gameObject = sourceObject;
            this.hitEffectPrefab = hitEffect;
            this.location = sourceObject?.transform.position ?? Vector3.zero;
            this.direction = direction.normalized;
            this.rotation = Quaternion.LookRotation(direction);
            this.maxDistance = maxDistance;
            IsActive = true;

            // Trigger OnStart event before performing the raycast
            OnStart?.Invoke(this);

            // Immediately perform the raycast
            PerformHitscan();
        }

        private void PerformHitscan()
        {
            if (Physics.Raycast(location, direction, out RaycastHit hitInfo, maxDistance, hitLayers))
            {
                // Hit something
                IUnit hitUnit = hitInfo.collider.GetComponent<IUnit>();
                
                // Spawn hit effect if prefab is provided
                if (hitEffectPrefab != null)
                {
                    GameObject hitEffect = UnityEngine.Object.Instantiate(
                        hitEffectPrefab, 
                        hitInfo.point, 
                        Quaternion.LookRotation(hitInfo.normal)
                    );
                    
                    // Clean up the effect after a delay
                    UnityEngine.Object.Destroy(hitEffect, 2.0f);
                }
                
                // Invoke hit callback
                onHit?.Invoke(this, hitInfo);
            }
            
            
            // Hitscan is a one-time operation
            IsActive = false;
        }

        public void Loop(ITime time)
        {
            // Hitscan is instant, so no per-frame behavior needed
            if (IsActive)
            {
                IsActive = false;
            }
        }

        public void Revoke()
        {
            IsActive = false;
            OnCompleted?.Invoke(this);
        }
    }
}