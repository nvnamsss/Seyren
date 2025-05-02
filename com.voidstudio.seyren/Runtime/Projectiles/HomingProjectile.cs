using System;
using Seyren.System.Units;
using Seyren.Universe;
using UnityEngine;

namespace Seyren.Projectiles
{
    public class HomingProjectile : IProjectile
    {
        public GameObject gameObject;
        public Vector3 location;
        public Quaternion rotation;
        public IUnit target;
        public float speed = 10f;
        public float lifeTime = 5f;

        private string _type = "Homing";
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
        private bool isActive = false;

        public Action<HomingProjectile> onHit; // Callback invoked on hit

        public HomingProjectile(IUnit target, GameObject prefab, float speed, float lifeTime)
        {
            gameObject = prefab;
            this.location = prefab.transform.position;
            this.rotation = prefab.transform.rotation;
            this.target = target;
            this.speed = speed;
            this.lifeTime = lifeTime;
            isActive = true;
        }

        public void Loop(ITime time)
        {
            if (target == null)
            {
                if (gameObject != null) UnityEngine.Object.Destroy(gameObject);
                isActive = false;
                return;
            }

            lifeTime -= time.DeltaTime;
            if (lifeTime <= 0f)
            {
                if (gameObject != null) UnityEngine.Object.Destroy(gameObject);
                isActive = false;
                return;
            }

            Vector3 targetPos = target.Location;
            Vector3 dir = (targetPos - location).normalized;
            float distance = (targetPos - location).magnitude;
            float move = speed * time.DeltaTime;

            if (distance <= move)
            {
                // Reached or passed the target
                location = targetPos;
                rotation = Quaternion.LookRotation(dir);
                if (gameObject != null)
                {
                    gameObject.transform.position = location;
                    gameObject.transform.rotation = rotation;
                }
                onHit?.Invoke(this);
                return;
            }

            location += dir * move;
            rotation = Quaternion.LookRotation(dir);

            if (gameObject != null)
            {
                gameObject.transform.position = location;
                gameObject.transform.rotation = rotation;
            }
        }

        public void Revoke()
        {
            isActive = false;
        }
    }
}
