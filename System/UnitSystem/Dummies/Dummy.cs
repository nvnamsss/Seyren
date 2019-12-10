using Crom.System.DamageSystem;
using Crom.System.UnitSystem;
using Crom.System.UnitSystem.Units;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Crom.System.UnitSystem.Dummies
{
    public class Dummy : MonoBehaviour, IAttribute, IObject
    {
        public int CustomValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Targetable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Invulnerable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        
        public bool IsFly { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Height { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float AnimationSpeed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float TurnSpeed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Color VertexColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Attribute Attribute { get; set; }

        public ModificationInfos Modification { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float HitDelay { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float TimeExpired { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Unit Owner { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Dummy()
        {

        }

        public void Damage(Unit target, DamageType type)
        {

        }

        public void Damage(Unit target, float damage, DamageType type)
        {

        }

        public static GameObject CreateDummy()
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

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void Hit()
        {
            throw new NotImplementedException();
        }
    }
}
