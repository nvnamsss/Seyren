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

        public static Dummy CreateCircleDummy()
        {
            GameObject go = new GameObject();
            SpriteRenderer render = go.AddComponent<SpriteRenderer>();
            go.AddComponent<CircleCollider2D>();
            go.AddComponent<Rigidbody2D>();
            Dummy dummy = go.AddComponent<Dummy>();

            return dummy;
        }

        public static Dummy CreateBoxDummy()
        {
            GameObject go = new GameObject();
            SpriteRenderer render = go.AddComponent<SpriteRenderer>();
            go.AddComponent<BoxCollider2D>();
            go.AddComponent<Rigidbody2D>();
            Dummy dummy = go.AddComponent<Dummy>();

            return dummy;
        }
    }
}
