using System.Collections;
using System.Collections.Generic;
using Base2D.System.ItemSystem;
using Base2D.System.UnitSystem.Units;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    #region  Singleton

    public static EquipmentManager instance;

    private void Awake() {
        instance = this;
    }

    #endregion

    public Hero character;

    Armor[] currentEquippedArmor;
    Weapon currentEquippedWeapon;

    public WeaponSlot weaponSlot;

    public ArmorSlot[] ArmorSlots;

    public Image weaponUI;

    public Sprite defaultWeapon;

    public SpriteRenderer characterWeapon;

    void Start(){
        int armorSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquippedArmor = new Armor[armorSlots];
    }

    public void Equip(Item newItem){
        Item oldItem = null;
        if(newItem.itemType == ItemType.Weapon){
            if (currentEquippedWeapon == null){
                currentEquippedWeapon = (Weapon)newItem;

                weaponSlot.weapon = (Weapon)newItem;
                weaponSlot.icon.sprite = currentEquippedWeapon.icon;
                weaponSlot.icon.enabled = true;
                weaponSlot.defaultIcon.enabled = false;

                weaponUI.sprite = currentEquippedWeapon.icon;

                characterWeapon.sprite = currentEquippedWeapon.icon;

               increaseStats(currentEquippedWeapon);
            }
            else{
                oldItem = currentEquippedWeapon;
                InventoryManager.instance.pickUp(oldItem);

                currentEquippedWeapon = (Weapon)newItem;

                weaponSlot.weapon = (Weapon)newItem;

                weaponSlot.icon.sprite = currentEquippedWeapon.icon;

                weaponUI.sprite = currentEquippedWeapon.icon;

                characterWeapon.sprite = currentEquippedWeapon.icon;

                switchStats(oldItem, currentEquippedWeapon);
            }
        }
        if(newItem.itemType == ItemType.Armor){
            int index = (int)((Armor)newItem).slot;
            if (currentEquippedArmor[index] == null){
                currentEquippedArmor[index] = (Armor)newItem;

                ArmorSlots[index].armor = (Armor)newItem;

                ArmorSlots[index].icon.sprite = currentEquippedArmor[index].icon;
                ArmorSlots[index].icon.enabled = true;
                ArmorSlots[index].defaultIcon.enabled = false;
                Debug.Log("equip");
                increaseStats(currentEquippedArmor[index]);
            }
            else{
                oldItem = currentEquippedArmor[index];
                InventoryManager.instance.pickUp(oldItem);

                currentEquippedArmor[index] = (Armor)newItem;

                ArmorSlots[index].armor = (Armor)newItem;

                ArmorSlots[index].icon.sprite = currentEquippedArmor[index].icon;

                switchStats(oldItem, currentEquippedArmor[index]);
            }
        }
    }

    public void UnequipWeapon(){
        InventoryManager.instance.pickUp(currentEquippedWeapon);
        decreaseStats(currentEquippedWeapon);
        currentEquippedWeapon = null;
        
        weaponSlot.icon.enabled = false;
        weaponSlot.defaultIcon.enabled = true;

        weaponUI.sprite = defaultWeapon;

        characterWeapon.sprite = defaultWeapon;
    }

    public void UnequipArmor(int index){
        InventoryManager.instance.pickUp(currentEquippedArmor[index]);
        decreaseStats(currentEquippedArmor[index]);
        currentEquippedArmor[index] = null;
        
        ArmorSlots[index].icon.enabled = false;
        ArmorSlots[index].defaultIcon.enabled = true;
    }

    public void increaseStats(Item item){
        Debug.Log("stats: "+item.Agility);
            character.Attribute.Strength += item.Strength;
            character.Attribute.Agility += item.Agility;
            character.Attribute.Intelligent +=item.Intelligent;

            character.Attribute.AttackDamage +=item.AttackDamage;
            character.Attribute.MDamageAmplified +=item.MDamageAmplified;

            character.Attribute.MaxHp +=item.MaxHp;
            character.Attribute.MaxMp +=item.MaxMp;
            character.Attribute.HpRegen +=item.HpRegen;
            character.Attribute.MpRegen +=item.MpRegen;
            character.Attribute.ShieldRegen +=item.ShieldRegen;
            character.Attribute.MShieldRegen +=item.MShieldRegen;
            character.Attribute.PShield +=item.PShield;
            character.Attribute.HpRegenPercent +=item.HpRegenPercent;
            character.Attribute.MpRegenPercent +=item.MpRegenPercent;

            character.Attribute.Armor +=item.Armor;
            character.Attribute.MArmor +=item.MArmor;

            character.Attribute.AttackRange +=item.AttackRange;
            character.Attribute.CastRange +=item.CastRange;

            character.Attribute.MovementSpeed +=item.MovementSpeed;
            character.Attribute.AttackSpeed +=item.AttackSpeed;
            character.Attribute.JumpSpeed +=item.JumpSpeed;

            StatusUIManager.instance.setStats();
    }

    public void decreaseStats(Item item){
        character.Attribute.Strength -= item.Strength;
            character.Attribute.Agility -= item.Agility;
            character.Attribute.Intelligent -=item.Intelligent;

            character.Attribute.AttackDamage -=item.AttackDamage;
            character.Attribute.MDamageAmplified -=item.MDamageAmplified;

            character.Attribute.MaxHp -=item.MaxHp;
            character.Attribute.MaxMp -=item.MaxMp;
            character.Attribute.HpRegen -=item.HpRegen;
            character.Attribute.MpRegen -=item.MpRegen;
            character.Attribute.ShieldRegen -=item.ShieldRegen;
            character.Attribute.MShieldRegen -=item.MShieldRegen;
            character.Attribute.PShield -=item.PShield;
            character.Attribute.HpRegenPercent -=item.HpRegenPercent;
            character.Attribute.MpRegenPercent -=item.MpRegenPercent;

            character.Attribute.Armor -=item.Armor;
            character.Attribute.MArmor -=item.MArmor;

            character.Attribute.AttackRange -=item.AttackRange;
            character.Attribute.CastRange -=item.CastRange;

            character.Attribute.MovementSpeed -=item.MovementSpeed;
            character.Attribute.AttackSpeed -=item.AttackSpeed;
            character.Attribute.JumpSpeed -=item.JumpSpeed;

            StatusUIManager.instance.setStats();
    }

    public void switchStats(Item oldItem, Item newItem){
         character.Attribute.Strength = character.Attribute.Strength - oldItem.Strength + newItem.Strength;
            character.Attribute.Agility = character.Attribute.Agility - oldItem.Agility + newItem.Agility;
            character.Attribute.Intelligent =character.Attribute.Intelligent - oldItem.Intelligent + newItem.Intelligent;

            character.Attribute.AttackDamage =character.Attribute.AttackDamage - oldItem.AttackDamage + newItem.AttackDamage;
            character.Attribute.MDamageAmplified =character.Attribute.MDamageAmplified - oldItem.MDamageAmplified + newItem.MDamageAmplified;

            character.Attribute.MaxHp =character.Attribute.MaxHp - oldItem.MaxHp + newItem.MaxHp;
            character.Attribute.MaxMp =character.Attribute.MaxMp - oldItem.MaxMp + newItem.MaxMp;
            character.Attribute.HpRegen =character.Attribute.HpRegen - oldItem.HpRegen + newItem.HpRegen;
            character.Attribute.MpRegen =character.Attribute.MpRegen - oldItem.MpRegen + newItem.MpRegen;
            character.Attribute.ShieldRegen =character.Attribute.ShieldRegen - oldItem.ShieldRegen + newItem.ShieldRegen;
            character.Attribute.MShieldRegen =character.Attribute.MShieldRegen - oldItem.MShieldRegen + newItem.MShieldRegen;
            character.Attribute.PShield =character.Attribute.PShield - oldItem.PShield + newItem.PShield;
            character.Attribute.HpRegenPercent =character.Attribute.HpRegenPercent - oldItem.HpRegenPercent + newItem.HpRegenPercent;
            character.Attribute.MpRegenPercent =character.Attribute.MpRegenPercent - oldItem.MpRegenPercent + newItem.MpRegenPercent;

            character.Attribute.Armor =character.Attribute.Armor - oldItem.Armor + newItem.Armor;
            character.Attribute.MArmor =character.Attribute.MArmor - oldItem.MArmor + newItem.MArmor;

            character.Attribute.AttackRange =character.Attribute.AttackRange - oldItem.AttackRange + newItem.AttackRange;
            character.Attribute.CastRange =character.Attribute.CastRange - oldItem.CastRange + newItem.CastRange;

            character.Attribute.MovementSpeed =character.Attribute.MovementSpeed - oldItem.MovementSpeed + newItem.MovementSpeed;
            character.Attribute.AttackSpeed =character.Attribute.AttackSpeed - oldItem.AttackSpeed + newItem.AttackSpeed;
            character.Attribute.JumpSpeed =character.Attribute.JumpSpeed - oldItem.JumpSpeed + newItem.JumpSpeed;

            StatusUIManager.instance.setStats();
    }

}
