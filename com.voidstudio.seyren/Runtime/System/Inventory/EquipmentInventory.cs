using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Seyren.System.Items;
using Seyren.System.Inventory;

namespace Seyren.System.Inventories
{
    /// <summary>
    /// Equipment slot types for character equipment
    /// </summary>
    public enum EquipmentSlot
    {
        Head = 0,
        Chest = 1,
        Belt = 2,
        Gloves = 3,
        Boots = 4,
        LeftHand = 5,
        RightHand = 6,
        Amulet = 7,
        LeftRing = 8,
        RightRing = 9
    }



    /// <summary>
    /// Equipment inventory implementation that provides specific slots for character equipment
    /// Implements IInventory interface for compatibility with existing inventory systems
    /// </summary>
    public class EquipmentInventory : IInventory
    {
        private const int EQUIPMENT_SLOT_COUNT = 10;
        private IItem[] equipmentSlots;
        private Dictionary<string, EquipmentSlot[]> itemTypeToSlots;

        // Events
        public event Action<IInventory> OnInventoryChanged;
        public event Action<IItem, int> OnItemAdded;
        public event Action<IItem, int> OnItemRemoved;
        public event Action<IInventory> OnInventoryFull;

        public EquipmentInventory()
        {
            equipmentSlots = new IItem[EQUIPMENT_SLOT_COUNT];
            InitializeItemTypeMapping();
        }



        private void InitializeItemTypeMapping()
        {
            itemTypeToSlots = new Dictionary<string, EquipmentSlot[]>
            {
                // Head equipment
                { "helmet", new[] { EquipmentSlot.Head } },
                { "hat", new[] { EquipmentSlot.Head } },
                { "crown", new[] { EquipmentSlot.Head } },
                { "circlet", new[] { EquipmentSlot.Head } },

                // Chest equipment
                { "armor", new[] { EquipmentSlot.Chest } },
                { "chest", new[] { EquipmentSlot.Chest } },
                { "robe", new[] { EquipmentSlot.Chest } },
                { "mail", new[] { EquipmentSlot.Chest } },

                // Belt equipment
                { "belt", new[] { EquipmentSlot.Belt } },
                { "sash", new[] { EquipmentSlot.Belt } },

                // Gloves equipment
                { "gloves", new[] { EquipmentSlot.Gloves } },
                { "gauntlets", new[] { EquipmentSlot.Gloves } },

                // Boots equipment
                { "boots", new[] { EquipmentSlot.Boots } },
                { "shoes", new[] { EquipmentSlot.Boots } },
                { "greaves", new[] { EquipmentSlot.Boots } },

                // Weapons (can go in either hand)
                { "weapon", new[] { EquipmentSlot.LeftHand, EquipmentSlot.RightHand } },
                { "sword", new[] { EquipmentSlot.LeftHand, EquipmentSlot.RightHand } },
                { "axe", new[] { EquipmentSlot.LeftHand, EquipmentSlot.RightHand } },
                { "mace", new[] { EquipmentSlot.LeftHand, EquipmentSlot.RightHand } },
                { "dagger", new[] { EquipmentSlot.LeftHand, EquipmentSlot.RightHand } },

                // Shields (typically right hand)
                { "shield", new[] { EquipmentSlot.RightHand, EquipmentSlot.LeftHand } },

                // Two-handed weapons (require both hands)
                { "bow", new[] { EquipmentSlot.LeftHand } }, // Will need special handling
                { "staff", new[] { EquipmentSlot.LeftHand } },
                { "spear", new[] { EquipmentSlot.LeftHand } },

                // Jewelry
                { "amulet", new[] { EquipmentSlot.Amulet } },
                { "necklace", new[] { EquipmentSlot.Amulet } },
                { "ring", new[] { EquipmentSlot.LeftRing, EquipmentSlot.RightRing } }
            };
        }

        #region IInventory Properties

        public int SlotCount => EQUIPMENT_SLOT_COUNT;

        public int OccupiedSlots => equipmentSlots.Count(item => item != null);

        public int EmptySlots => equipmentSlots.Count(item => item == null);

        public bool IsFull => EmptySlots == 0;

        public bool IsEmpty => OccupiedSlots == 0;

        #endregion

        #region IInventory Core Methods

        public IItem GetSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= EQUIPMENT_SLOT_COUNT)
                return null;
            return equipmentSlots[slotIndex];
        }

        public void SetSlot(int slotIndex, IItem item)
        {
            if (slotIndex < 0 || slotIndex >= EQUIPMENT_SLOT_COUNT)
                return;

            var oldItem = equipmentSlots[slotIndex];
            equipmentSlots[slotIndex] = item;

            // Fire events
            if (oldItem != null)
            {
                OnItemRemoved?.Invoke(oldItem, oldItem.Count);
            }

            if (item != null)
            {
                OnItemAdded?.Invoke(item, item.Count);
            }

            OnInventoryChanged?.Invoke(this);
        }

        public IItem[] GetAllSlots()
        {
            return (IItem[])equipmentSlots.Clone();
        }

        #endregion

        #region Equipment-Specific Methods

        /// <summary>
        /// Get equipped item from specific equipment slot
        /// </summary>
        public IItem GetEquippedItem(EquipmentSlot slot)
        {
            var slotIndex = (int)slot;
            return GetSlot(slotIndex);
        }

        /// <summary>
        /// Equip item to specific slot
        /// </summary>
        public bool EquipItem(IItem item, EquipmentSlot slot)
        {
            if (item == null) return false;

            // Check if item can be equipped in this slot
            if (!CanEquipItemInSlot(item, slot))
                return false;

            var slotIndex = (int)slot;
            var currentItem = equipmentSlots[slotIndex];

            // Unequip existing item
            if (currentItem != null)
            {
                UnequipItem(slot);
            }

            // Equip new item
            SetSlot(slotIndex, item);

            return true;
        }

        /// <summary>
        /// Unequip item from specific slot
        /// </summary>
        public IItem UnequipItem(EquipmentSlot slot)
        {
            var slotIndex = (int)slot;
            var currentItem = equipmentSlots[slotIndex];
            
            if (currentItem == null)
                return null;

            var item = currentItem;
            SetSlot(slotIndex, null);
            
            return item;
        }

        /// <summary>
        /// Check if item can be equipped in the specified slot
        /// </summary>
        public bool CanEquipItemInSlot(IItem item, EquipmentSlot slot)
        {
            if (item == null) return false;

            // Check if the item type can be equipped in this slot
            var validSlots = GetValidSlotsForItem(item);
            return validSlots.Contains(slot);
        }

        /// <summary>
        /// Get all valid slots where this item can be equipped
        /// </summary>
        public EquipmentSlot[] GetValidSlotsForItem(IItem item)
        {
            if (item == null) return new EquipmentSlot[0];

            var validSlots = new List<EquipmentSlot>();

            foreach (var mapping in itemTypeToSlots)
            {
                if (item.TypeId.ToLower().Contains(mapping.Key.ToLower()))
                {
                    validSlots.AddRange(mapping.Value);
                }
            }

            return validSlots.Distinct().ToArray();
        }

        /// <summary>
        /// Get all equipped items as a dictionary
        /// </summary>
        public Dictionary<EquipmentSlot, IItem> GetAllEquippedItems()
        {
            var result = new Dictionary<EquipmentSlot, IItem>();
            
            for (int i = 0; i < EQUIPMENT_SLOT_COUNT; i++)
            {
                var slot = (EquipmentSlot)i;
                var item = GetEquippedItem(slot);
                if (item != null)
                {
                    result[slot] = item;
                }
            }

            return result;
        }

        #endregion

        #region IInventory Add/Remove Methods

        public AddItemResult AddItem(IItem item)
        {
            if (item == null || item.Count <= 0)
                return new AddItemResult(new List<IItem>(), 0, ItemKind.Unique);

            // For equipment, we only support adding one item at a time
            // If item.Count > 1, we still only equip one
            var validSlots = GetValidSlotsForItem(item);
            if (validSlots.Length == 0)
                return new AddItemResult(new List<IItem>(), 0, ItemKind.Unique);

            // Find first empty valid slot
            foreach (var slot in validSlots)
            {
                var slotIndex = (int)slot;
                var currentItem = equipmentSlots[slotIndex];
                
                if (currentItem == null)
                {
                    if (EquipItem(item, slot))
                    {
                        return new AddItemResult(
                            new List<IItem> { item }, 
                            1, 
                            ItemKind.Unique);
                    }
                }
            }

            // No available slots
            OnInventoryFull?.Invoke(this);
            return new AddItemResult(new List<IItem>(), 0, ItemKind.Unique);
        }

        public AddItemResult RemoveItem(IItem item)
        {
            if (item == null)
                return new AddItemResult(new List<IItem>(), 0, ItemKind.Unique);

            return RemoveItem(item.ID);
        }

        public AddItemResult RemoveItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
                return new AddItemResult(new List<IItem>(), 0, ItemKind.Unique);

            var removedItems = new List<IItem>();
            int totalRemoved = 0;

            for (int i = 0; i < EQUIPMENT_SLOT_COUNT; i++)
            {
                var item = equipmentSlots[i];
                if (item != null && item.ID == itemId)
                {
                    removedItems.Add(item);
                    SetSlot(i, null);
                    totalRemoved++;
                }
            }

            return new AddItemResult(removedItems, totalRemoved, ItemKind.Unique);
        }

        #endregion

        #region IInventory Query Methods

        public bool ContainsItem(IItem item)
        {
            return item != null && ContainsItem(item.ID);
        }

        public bool ContainsItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
                return false;

            return equipmentSlots.Any(item => item != null && item.ID == itemId);
        }

        public bool ContainsItem(string itemId, int quantity)
        {
            if (string.IsNullOrEmpty(itemId))
                return false;

            int found = 0;
            foreach (var item in equipmentSlots)
            {
                if (item != null && item.ID == itemId)
                {
                    found++;
                    if (found >= quantity)
                        return true;
                }
            }
            return false;
        }

        public int GetItemCountByTypeID(IItem item)
        {
            return item != null ? GetItemCountByTypeID(item.TypeId) : 0;
        }

        public int GetItemCountByTypeID(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
                return 0;

            int count = 0;
            foreach (var item in equipmentSlots)
            {
                if (item != null && 
                    (item.ID == itemId || item.TypeId == itemId))
                {
                    count += item.Count;
                }
            }
            return count;
        }

        public bool CanAddItem(IItem item)
        {
            if (item == null) return false;

            var validSlots = GetValidSlotsForItem(item);
            foreach (var slot in validSlots)
            {
                var slotIndex = (int)slot;
                var currentItem = equipmentSlots[slotIndex];
                if (currentItem == null)
                    return true;
            }
            return false;
        }

        #endregion

        #region IInventory Find Methods

        public int FindItemSlot(IItem item)
        {
            return item != null ? FindItemSlot(item.ID) : -1;
        }

        public int FindItemSlot(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
                return -1;

            for (int i = 0; i < EQUIPMENT_SLOT_COUNT; i++)
            {
                var item = equipmentSlots[i];
                if (item != null && item.ID == itemId)
                    return i;
            }
            return -1;
        }

        public int[] FindAllItemSlots(string itemTypeId)
        {
            if (string.IsNullOrEmpty(itemTypeId))
                return new int[0];

            var slots = new List<int>();
            for (int i = 0; i < EQUIPMENT_SLOT_COUNT; i++)
            {
                var currentItem = equipmentSlots[i];
                if (currentItem != null && currentItem.TypeId == itemTypeId)
                    slots.Add(i);
            }
            return slots.ToArray();
        }

        public int FindEmptySlot()
        {
            for (int i = 0; i < EQUIPMENT_SLOT_COUNT; i++)
            {
                var item = equipmentSlots[i];
                if (item == null)
                    return i;
            }
            return -1;
        }

        public int[] FindAllEmptySlots()
        {
            var slots = new List<int>();
            for (int i = 0; i < EQUIPMENT_SLOT_COUNT; i++)
            {
                var item = equipmentSlots[i];
                if (item == null)
                    slots.Add(i);
            }
            return slots.ToArray();
        }

        #endregion

        #region IInventory Manipulation Methods

        public void SwapSlots(int slotA, int slotB)
        {
            if (slotA < 0 || slotA >= EQUIPMENT_SLOT_COUNT ||
                slotB < 0 || slotB >= EQUIPMENT_SLOT_COUNT)
                return;

            var tempStack = equipmentSlots[slotA];
            equipmentSlots[slotA] = equipmentSlots[slotB];
            equipmentSlots[slotB] = tempStack;

            OnInventoryChanged?.Invoke(this);
        }

        public int MoveItems(int fromSlot, int toSlot, int quantity = 0)
        {
            if (fromSlot < 0 || fromSlot >= EQUIPMENT_SLOT_COUNT ||
                toSlot < 0 || toSlot >= EQUIPMENT_SLOT_COUNT ||
                fromSlot == toSlot)
                return 0;

            var fromItem = equipmentSlots[fromSlot];
            if (fromItem == null)
                return 0;

            var toItem = equipmentSlots[toSlot];
            if (toItem != null)
                return 0; // Equipment slots can only hold one item

            // Move the entire item
            equipmentSlots[toSlot] = fromItem;
            equipmentSlots[fromSlot] = null;

            OnInventoryChanged?.Invoke(this);
            return 1;
        }

        public void Clear()
        {
            for (int i = 0; i < EQUIPMENT_SLOT_COUNT; i++)
            {
                var item = equipmentSlots[i];
                if (item != null)
                {
                    OnItemRemoved?.Invoke(item, item.Count);
                }
                equipmentSlots[i] = null;
            }
            OnInventoryChanged?.Invoke(this);
        }

        public void ClearSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= EQUIPMENT_SLOT_COUNT)
                return;

            var item = equipmentSlots[slotIndex];
            if (item != null)
            {
                OnItemRemoved?.Invoke(item, item.Count);
            }
            
            equipmentSlots[slotIndex] = null;
            OnInventoryChanged?.Invoke(this);
        }

        #endregion

        #region IInventory Utility Methods

        public List<IItem> GetUniqueItems()
        {
            var uniqueItems = new List<IItem>();
            var seenIds = new HashSet<string>();

            foreach (var item in equipmentSlots)
            {
                if (item != null && !seenIds.Contains(item.ID))
                {
                    uniqueItems.Add(item);
                    seenIds.Add(item.ID);
                }
            }

            return uniqueItems;
        }

        public List<IItem> GetItemsByRarity(int rarity)
        {
            var items = new List<IItem>();
            foreach (var item in equipmentSlots)
            {
                if (item != null && item.Rarity == rarity)
                {
                    items.Add(item);
                }
            }
            return items;
        }

        public void SortInventory(InventorySortType sortType)
        {
            // Equipment inventory doesn't need traditional sorting
            // as each slot has a specific purpose
            // This could be implemented to reorganize by rarity or other criteria if needed
        }

        public void CompactInventory()
        {
            // Equipment inventory doesn't need compacting
            // as each slot holds exactly one item
        }

        #endregion

        #region IInventory Serialization Methods

        public InventoryData GetInventoryData()
        {
            var data = new InventoryData();
            data.slots = new InventoryData.ItemStackData[EQUIPMENT_SLOT_COUNT];

            for (int i = 0; i < EQUIPMENT_SLOT_COUNT; i++)
            {
                var item = equipmentSlots[i];
                if (item != null)
                {
                    data.slots[i] = new InventoryData.ItemStackData
                    {
                        itemId = item.ID,
                        quantity = item.Count
                    };
                }
            }

            return data;
        }

        public void LoadInventoryData(InventoryData data)
        {
            if (data?.slots == null)
                return;

            Clear();

            for (int i = 0; i < Math.Min(data.slots.Length, EQUIPMENT_SLOT_COUNT); i++)
            {
                var slotData = data.slots[i];
                if (slotData != null && !string.IsNullOrEmpty(slotData.itemId))
                {
                    // Note: This would need to be connected to an item database
                    // to recreate items from their IDs
                    // For now, we'll leave the slot empty
                    Debug.LogWarning($"Cannot load equipment item {slotData.itemId} - item database integration needed");
                }
            }
        }

        #endregion
    }
}