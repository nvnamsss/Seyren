﻿using UnityEngine;

namespace Base2D.System.ItemSystem
{
    public class Weapon : Item
    {
        void OnAwake(){
            this.itemType = ItemType.Weapon;
        }

        public WeaponType weaponType;

        public override void Use(){
            EquipmentManager.instance.Equip(this);
            RemoveFromInventory(this);
        }
    }

    public enum WeaponType{
        Sword,
        Spear
    }
}
