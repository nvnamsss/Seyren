using UnityEngine;

namespace Base2D.System.ItemSystem
{
    public class Consumables : Item
    {
        void OnAwake(){
            this.itemType = ItemType.Consumable;
        }
    }
}
