using UnityEngine;

namespace Base2D.System.ItemSystem
{
    public class Armor : Item
    {
        void OnAwake(){
            this.itemType = ItemType.Armor;
        }

        public EquipmentSlot slot;

        public override void Use(){
            EquipmentManager.instance.Equip(this);
            RemoveFromInventory(this);
        }
    }

    public enum EquipmentSlot{
        Head,
        Top,
        Bottom
    }
}
