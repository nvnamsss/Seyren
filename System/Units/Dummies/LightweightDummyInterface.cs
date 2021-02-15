using UnityEngine;

namespace Seyren.System.Units.Dummies
{
    public partial class LightweightDummy : MonoBehaviour
    {
        /// <summary>
        /// Create dummy base on existed GameObject, based Object must contain RigidBody2D and Collider2D
        /// </summary>
        public static LightweightDummy Create(GameObject go)
        {
            GameObject g = Instantiate(go);
            LightweightDummy dummy = g.AddComponent<LightweightDummy>();

            return dummy;
        }

        /// <summary>
        /// Create dummy base on existed GameObject then add Collider to GameObject
        /// </summary>
        /// <typeparam name="TCollider2D">Collider2D type like BoxCollider2D, CircleCollider2D, etc</typeparam>
        public static LightweightDummy Create<TCollider2D>(GameObject go) where TCollider2D : Collider2D
        {
            GameObject g = Instantiate(go);
            g.name = go.name;
            Rigidbody2D body = g.GetComponent<Rigidbody2D>();
            TCollider2D collider = g.GetComponent<TCollider2D>();
            if (body == null) g.AddComponent<Rigidbody2D>();
            if (collider == null) g.AddComponent<TCollider2D>();
            LightweightDummy dummy = g.AddComponent<LightweightDummy>();

            return dummy;
        }
    }
}
