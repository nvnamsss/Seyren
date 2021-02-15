using UnityEngine;

namespace Seyren.System.Items
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
