using UnityEngine;

namespace Base2D.System.ItemSystem
{
    public class Weapon : Item
    {
        void OnAwake(){
            this.itemType = ItemType.Weapon;
        }
    }
}
