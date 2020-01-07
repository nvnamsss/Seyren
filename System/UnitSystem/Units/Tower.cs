using UnityEngine;

namespace Base2D.System.UnitSystem.Units
{
    public class Tower : Unit
    {
        public new static Unit Create(string name, Vector3 location, Quaternion rotation, Sprite sprite)
        {
            GameObject go = CreateObject(name, location, rotation, sprite);
            var tower = go.AddComponent<Tower>();
            tower.Attribute.MovementSpeed = 0;
            return tower;
        }

    }
}
