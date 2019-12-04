using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Crom.System.UnitSystem.Projectiles
{
    public static class ProjectileInterface
    {
        public static Projectile CreateProjectile(string name, string imagePath)
        {
            GameObject go = new GameObject(name);
            SpriteRenderer render = go.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            Projectile projectile = go.AddComponent(typeof(Projectile)) as Projectile;
            go.AddComponent(typeof(Rigidbody2D));
            go.AddComponent(typeof(BoxCollider2D));

            Texture2D texture = new Texture2D(512, 256);
            byte[] data = File.ReadAllBytes(imagePath);
            texture.LoadImage(data);
            render.sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(512, 256)), new Vector2(0, 0));
            return projectile;
        }

        public static void SetProjectileType(Projectile projectile, ProjectileType type)
        {
            projectile.Type = type;
        }

        //public static void SetProjectileSpeed(Projectile projectile, double speed)
        //{
        //    projectile.Speed = speed;
        //}
    }
}
