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
        private bool isActive = false;

        public Action<CommonProjectile> onTick;

        public CommonProjectile(GameObject gameObject, Vector3 direction, float speed, float lifeTime)
        {
            this.gameObject = gameObject;
            this.location = gameObject.transform.position;
            this.direction = direction.normalized;
            this.speed = speed;
            this.lifeTime = lifeTime;
            this.rotation = Quaternion.LookRotation(this.direction);
            gameObject.transform.rotation = rotation;
            isActive = true;
        }

        public void Loop(ITime time)
        {
            lifeTime -= time.DeltaTime;
            if (lifeTime <= 0f)
            {
                if (gameObject != null) UnityEngine.Object.Destroy(gameObject);
                isActive = false;
                return;
            }

            location += direction * speed * time.DeltaTime;
            if (gameObject != null)
            {
                gameObject.transform.position = location;
            }
            onTick?.Invoke(this);
        }

        public void Revoke()
        {
            isActive = false;
        }
    }
}
