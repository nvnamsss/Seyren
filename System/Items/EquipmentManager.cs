using System.Collections;
using System.Collections.Generic;
using Seyren.System.Items;
using Seyren.System.Units;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    #region  Singleton

    public static EquipmentManager instance;

    private void Awake()
    {
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

    void Start()
    {
        int armorSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquippedArmor = new Armor[armorSlots];
    }

    public void Equip(Item newItem)
    {
        Item oldItem = null;
        if (newItem.itemType == ItemType.Weapon)
        {
            if (currentEquippedWeapon == null)
            {
                currentEquippedWeapon = (Weapon)newItem;

                weaponSlot.weapon = (Weapon)newItem;
                weaponSlot.icon.sprite = currentEquippedWeapon.icon;
                weaponSlot.icon.enabled = true;
                weaponSlot.defaultIcon.enabled = false;

                weaponUI.sprite = currentEquippedWeapon.icon;

                characterWeapon.sprite = currentEquippedWeapon.icon;

                increaseStats(currentEquippedWeapon);
            }
            else
            {
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
        if (newItem.itemType == ItemType.Armor)
        {
            int index = (int)((Armor)newItem).slot;
            if (currentEquippedArmor[index] == null)
            {
                currentEquippedArmor[index] = (Armor)newItem;

                ArmorSlots[index].armor = (Armor)newItem;

                ArmorSlots[index].icon.sprite = currentEquippedArmor[index].icon;
                ArmorSlots[index].icon.enabled = true;
                ArmorSlots[index].defaultIcon.enabled = false;
                Debug.Log("equip");
                increaseStats(currentEquippedArmor[index]);
            }
            else
            {
                oldItem = currentEquippedArmor[index];
                InventoryManager.instance.pickUp(oldItem);

                currentEquippedArmor[index] = (Armor)newItem;

                ArmorSlots[index].armor = (Armor)newItem;

                ArmorSlots[index].icon.sprite = currentEquippedArmor[index].icon;

                switchStats(oldItem, currentEquippedArmor[index]);
            }
        }
    }

    public void UnequipWeapon()
    {
        InventoryManager.instance.pickUp(currentEquippedWeapon);
        decreaseStats(currentEquippedWeapon);
        currentEquippedWeapon = null;

        weaponSlot.icon.enabled = false;
        weaponSlot.defaultIcon.enabled = true;

        weaponUI.sprite = defaultWeapon;

        characterWeapon.sprite = defaultWeapon;
    }

    public void UnequipArmor(int index)
    {
        InventoryManager.instance.pickUp(currentEquippedArmor[index]);
        decreaseStats(currentEquippedArmor[index]);
        currentEquippedArmor[index] = null;

        ArmorSlots[index].icon.enabled = false;
        ArmorSlots[index].defaultIcon.enabled = true;
    }

    public void increaseStats(Item item)
    {
        character.Attribute.Strength += item.Attribute.Strength;
        character.Attribute.Agility += item.Attribute.Agility;
        character.Attribute.Intelligent += item.Attribute.Intelligent;

        character.Attribute.AttackDamage += item.Attribute.AttackDamage;
        character.Attribute.MDamageAmplified += item.Attribute.MDamageAmplified;

        character.Attribute.MaxHp += item.Attribute.MaxHp;
        character.Attribute.MaxMp += item.Attribute.MaxMp;
        character.Attribute.HpRegen += item.Attribute.HpRegen;
        character.Attribute.MpRegen += item.Attribute.MpRegen;
        character.Attribute.ShieldRegen += item.Attribute.ShieldRegen;
        character.Attribute.MShieldRegen += item.Attribute.MShieldRegen;
        character.Attribute.PShield += item.Attribute.PShield;
        character.Attribute.HpRegenPercent += item.Attribute.HpRegenPercent;
        character.Attribute.MpRegenPercent += item.Attribute.MpRegenPercent;

        character.Attribute.Armor += item.Attribute.Armor;
        character.Attribute.MArmor += item.Attribute.MArmor;

        character.Attribute.AttackRange += item.Attribute.AttackRange;
        character.Attribute.CastRange += item.Attribute.CastRange;

        character.Attribute.MovementSpeed += item.Attribute.MovementSpeed;
        character.Attribute.AttackSpeed += item.Attribute.AttackSpeed;
        character.Attribute.JumpSpeed += item.Attribute.JumpSpeed;

        StatusUIManager.instance.setStats();
    }

    public void decreaseStats(Item item)
    {
        character.Attribute.Strength -= item.Attribute.Strength;
        character.Attribute.Agility -= item.Attribute.Agility;
        character.Attribute.Intelligent -= item.Attribute.Intelligent;

        character.Attribute.AttackDamage -= item.Attribute.AttackDamage;
        character.Attribute.MDamageAmplified -= item.Attribute.MDamageAmplified;

        character.Attribute.MaxHp -= item.Attribute.MaxHp;
        character.Attribute.MaxMp -= item.Attribute.MaxMp;
        character.Attribute.HpRegen -= item.Attribute.HpRegen;
        character.Attribute.MpRegen -= item.Attribute.MpRegen;
        character.Attribute.ShieldRegen -= item.Attribute.ShieldRegen;
        character.Attribute.MShieldRegen -= item.Attribute.MShieldRegen;
        character.Attribute.PShield -= item.Attribute.PShield;
        character.Attribute.HpRegenPercent -= item.Attribute.HpRegenPercent;
        character.Attribute.MpRegenPercent -= item.Attribute.MpRegenPercent;

        character.Attribute.Armor -= item.Attribute.Armor;
        character.Attribute.MArmor -= item.Attribute.MArmor;

        character.Attribute.AttackRange -= item.Attribute.AttackRange;
        character.Attribute.CastRange -= item.Attribute.CastRange;

        character.Attribute.MovementSpeed -= item.Attribute.MovementSpeed;
        character.Attribute.AttackSpeed -= item.Attribute.AttackSpeed;
        character.Attribute.JumpSpeed -= item.Attribute.JumpSpeed;

        StatusUIManager.instance.setStats();
    }

    public void switchStats(Item oldItem, Item newItem)
    {
        character.Attribute.Strength = character.Attribute.Strength - oldItem.Attribute.Strength + newItem.Attribute.Strength;
        character.Attribute.Agility = character.Attribute.Agility - oldItem.Attribute.Agility + newItem.Attribute.Agility;
        character.Attribute.Intelligent = character.Attribute.Intelligent - oldItem.Attribute.Intelligent + newItem.Attribute.Intelligent;

        character.Attribute.AttackDamage = character.Attribute.AttackDamage - oldItem.Attribute.AttackDamage + newItem.Attribute.AttackDamage;
        character.Attribute.MDamageAmplified = character.Attribute.MDamageAmplified - oldItem.Attribute.MDamageAmplified + newItem.Attribute.MDamageAmplified;

        character.Attribute.MaxHp = character.Attribute.MaxHp - oldItem.Attribute.MaxHp + newItem.Attribute.MaxHp;
        character.Attribute.MaxMp = character.Attribute.MaxMp - oldItem.Attribute.MaxMp + newItem.Attribute.MaxMp;
        character.Attribute.HpRegen = character.Attribute.HpRegen - oldItem.Attribute.HpRegen + newItem.Attribute.HpRegen;
        character.Attribute.MpRegen = character.Attribute.MpRegen - oldItem.Attribute.MpRegen + newItem.Attribute.MpRegen;
        character.Attribute.ShieldRegen = character.Attribute.ShieldRegen - oldItem.Attribute.ShieldRegen + newItem.Attribute.ShieldRegen;
        character.Attribute.MShieldRegen = character.Attribute.MShieldRegen - oldItem.Attribute.MShieldRegen + newItem.Attribute.MShieldRegen;
        character.Attribute.PShield = character.Attribute.PShield - oldItem.Attribute.PShield + newItem.Attribute.PShield;
        character.Attribute.HpRegenPercent = character.Attribute.HpRegenPercent - oldItem.Attribute.HpRegenPercent + newItem.Attribute.HpRegenPercent;
        character.Attribute.MpRegenPercent = character.Attribute.MpRegenPercent - oldItem.Attribute.MpRegenPercent + newItem.Attribute.MpRegenPercent;

        character.Attribute.Armor = character.Attribute.Armor - oldItem.Attribute.Armor + newItem.Attribute.Armor;
        character.Attribute.MArmor = character.Attribute.MArmor - oldItem.Attribute.MArmor + newItem.Attribute.MArmor;

        character.Attribute.AttackRange = character.Attribute.AttackRange - oldItem.Attribute.AttackRange + newItem.Attribute.AttackRange;
        character.Attribute.CastRange = character.Attribute.CastRange - oldItem.Attribute.CastRange + newItem.Attribute.CastRange;

        character.Attribute.MovementSpeed = character.Attribute.MovementSpeed - oldItem.Attribute.MovementSpeed + newItem.Attribute.MovementSpeed;
        character.Attribute.AttackSpeed = character.Attribute.AttackSpeed - oldItem.Attribute.AttackSpeed + newItem.Attribute.AttackSpeed;
        character.Attribute.JumpSpeed = character.Attribute.JumpSpeed - oldItem.Attribute.JumpSpeed + newItem.Attribute.JumpSpeed;

        StatusUIManager.instance.setStats();
    }

}
