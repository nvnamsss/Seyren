using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Seyren.System.Units
{
    public partial class Unit : IUnit, IAttribute
    {
        public static Unit Create(string name, Vector3 location, Quaternion rotation, Sprite sprite)
        {
            Unit unit = new Unit();
            return unit;
            // GameObject go = CreateObject(name, location, rotation, sprite);
            // var unit = go.AddComponent<Unit>();

            // return unit;
        }

        public static Unit Create(GameObject go)
        {
            Unit unit = new Unit();
            return unit;
            // GameObject g = Instantiate(go);
            // Unit unit = g.AddComponent<Unit>();

            // return unit;
        }

        public static Unit Create<TCollider2D>(GameObject go) where TCollider2D : Collider2D
        {
            // GameObject g = Instantiate(go);
            // TCollider2D collider = g.GetComponent<TCollider2D>();
            // if (collider == null) g.AddComponent<TCollider2D>();
            // Unit unit = g.AddComponent<Unit>();
            Unit unit = new Unit();
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

        public static GameObject CreateShadow(Unit u, float transparent)
        {
            return null;
            // GameObject go = new GameObject(u.name + "- shadow");
            // go.transform.position = Vector3.zero;
            // SpriteRenderer[] objs = u.gameObject.GetComponentsInChildren<SpriteRenderer>(false);

            // for (int loop = 0; loop < objs.Length; loop++)
            // {
            //     //SpriteRenderer renderer = objs[loop].GetComponent<SpriteRenderer>();
            //     SpriteRenderer renderer = objs[loop];
            //     GameObject child = new GameObject(renderer.name);
            //     SpriteRenderer cr = child.AddComponent<SpriteRenderer>();

            //     cr.sprite = renderer.sprite;
            //     cr.sortingOrder = renderer.sortingOrder;
            //     cr.flipX = renderer.flipX;
            //     cr.flipY = renderer.flipY;
            //     cr.material = renderer.material;
            //     cr.drawMode = renderer.drawMode;
            //     cr.sortingLayerID = renderer.sortingLayerID;
            //     cr.spriteSortPoint = renderer.spriteSortPoint;
            //     cr.maskInteraction = renderer.maskInteraction;
            //     //cr.sprite = renderer.sprite;
            //     child.transform.position = renderer.transform.position;
            //     child.transform.rotation = renderer.transform.rotation;
            //     child.transform.localScale = renderer.transform.lossyScale;
            //     child.transform.SetParent(go.transform);

            //     Color color = cr.material.color;
            //     color.a *= 0.6f;
            //     cr.material.color = color;
            // }

            // return go;
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
