using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base2D.System.UnitSystem.Dummies
{
    public partial class Dummy : MonoBehaviour, IAttribute, IObject
    {
        public static GameObject Create()
        {
            GameObject go = new GameObject();
            SpriteRenderer render = go.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            Texture2D texture = new Texture2D(512, 256);
            byte[] data = File.ReadAllBytes(Path.Combine(Application.dataPath, "Knight Files", "Knight PNG", "Knight_attack_01.png"));

            go.AddComponent(typeof(Dummy));
            texture.LoadImage(data);
            render.sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(512, 256)), new Vector2(0, 0));
            return go;
        }

        /// <summary>
        /// Create dummy base on existed GameObject, based Object must contain RigidBody2D and Collider2D
        /// </summary>
        public static Dummy Create(GameObject go)
        {
            GameObject g = Instantiate(go);
            Dummy dummy = g.AddComponent<Dummy>();

            return dummy;
        }

        /// <summary>
        /// Create dummy base on existed GameObject then add Collider to GameObject
        /// </summary>
        /// <typeparam name="TCollider2D">Collider2D type like BoxCollider2D, CircleCollider2D, etc</typeparam>
        public static Dummy Create<TCollider2D>(GameObject go) where TCollider2D : Collider2D
        {
            GameObject g = Instantiate(go);
            g.name = go.name;
            Rigidbody2D body = g.GetComponent<Rigidbody2D>();
            TCollider2D collider = g.GetComponent<TCollider2D>();
            if (body == null) g.AddComponent<Rigidbody2D>();
            if (collider == null) g.AddComponent<TCollider2D>();
            Dummy dummy = g.AddComponent<Dummy>();

            return dummy;
        }

    }
}
