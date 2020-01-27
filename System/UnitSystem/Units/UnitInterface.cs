using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base2D.System.UnitSystem.Units
{
    public partial class Unit : MonoBehaviour, IObject, IAttribute
    {
        public static Unit Create(string name, Vector3 location, Quaternion rotation, Sprite sprite)
        {
            GameObject go = CreateObject(name, location, rotation, sprite);
            var unit = go.AddComponent<Unit>();

            return unit;
        }

        public static Unit Create(GameObject go)
        {
            GameObject g = Instantiate(go);
            Unit unit = g.AddComponent<Unit>();

            return unit;
        }

        public static Unit Create<TCollider2D>(GameObject go) where TCollider2D : Collider2D
        {
            GameObject g = Instantiate(go);
            TCollider2D collider = g.GetComponent<TCollider2D>();
            if (collider == null) g.AddComponent<TCollider2D>();
            Unit unit = g.AddComponent<Unit>();

            return unit;
        }

        protected static GameObject CreateObject(string name, Vector3 location, Quaternion rotation, Sprite sprite)
        {
            GameObject go = new GameObject(name);
            SpriteRenderer render = go.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            go.transform.position = location;
            go.transform.rotation = rotation;
            render.sprite = sprite;
            return go;
        }

        public static bool IsEnemy(Unit unit1, Unit unit2)
        {
            return true;
        }

        public static void Pause(Unit target, bool pause)
        {
            if (pause)
            {
                target.TimeScale = 0;
            }
            else
            {
                target.TimeScale = 1.0f;
            }
        }

        public static UnityEngine.GameObject CreateUnit()
        {
            return null;
        }

        public static void AddAbility(Unit unit)
        {

        }

        public static void RemoveAbility(Unit unit)
        {

        }

        public static void SetMovementSpeed(Unit unit, float speed)
        {

        }

        public static void KillUnit(Unit unit)
        {

        }

        public static void RevivetUnit(Unit unit)
        {

        }
    }
}
