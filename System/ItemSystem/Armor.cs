using UnityEngine;

namespace Base2D.System.ItemSystem
{
    public class Armor : Item
    {
        void OnAwake(){
            this.itemType = ItemType.Armor;
        }
    }
}
