using System;
using System.Collections.Generic;
using UnityEngine;
using Seyren.System.Items;

namespace Seyren.System.Inventory
{
    // Using IItem from Seyren.System.Items namespace
    // IItem interface already exists with:
    // - string ID { get; set; }
    // - string Name { get; set; }
    // - string Description { get; set; }
    // - int Count { get; set; }
    // - int Width { get; set; }
    // - int Height { get; set; }
    // - int Rarity { get; set; }

    /// <summary>
    /// Represents the kind of item being added to inventory
    /// </summary>
    public enum ItemKind
    {
        /// <summary>
        /// Item that cannot be stacked (MaxStack = 1)
        /// </summary>
        Unique,
        /// <summary>
        /// Item that can be stacked (MaxStack > 1)
        /// </summary>
        Stackable
    }

    /// <summary>
    /// Result returned from AddItem operation providing detailed information about the operation
    /// </summary>
    public struct AddItemResult
    {
        /// <summary>
        /// List of new item instances created during the operation
        /// </summary>
        public List<IItem> NewItemsCreated { get; set; }
        
        /// <summary>
        /// Total number of items added to the inventory
        /// </summary>
        public int TotalAdded { get; set; }
        
        /// <summary>
        /// The kind of item that was added (Unique or Stackable)
        /// </summary>
        public ItemKind Kind { get; set; }

        public AddItemResult(List<IItem> newItemsCreated, int totalAdded, ItemKind kind)
        {
            NewItemsCreated = newItemsCreated ?? new List<IItem>();
            TotalAdded = totalAdded;
            Kind = kind;
        }
    }
    // - int MaxStack { get; set; }
    // - void Use(UseItemData data)

    /// <summary>
    /// Represents a stack of items in an inventory slot
    /// </summary>
    public interface IItemStack
    {
        /// <summary>
        /// The item type in this stack
        /// </summary>
        IItem Item { get; }
        
        /// <summary>
        /// Current quantity in this stack
        /// </summary>
        int Quantity { get; set; }
        
        /// <summary>
        /// Whether this stack is empty
        /// </summary>
        bool IsEmpty { get; }
        
        /// <summary>
        /// Whether this stack is at maximum capacity
        /// </summary>
        bool IsFull { get; }
        
        /// <summary>
        /// Remaining space in this stack
        /// </summary>
        int RemainingSpace { get; }
        
        /// <summary>
        /// Check if items can be added to this stack
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <param name="quantity">Quantity to add</param>
        /// <returns>True if items can be added</returns>
        bool CanAddItems(IItem item, int quantity);
        
        /// <summary>
        /// Add items to this stack
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <param name="quantity">Quantity to add</param>
        /// <returns>Quantity actually added</returns>
        int AddItems(IItem item, int quantity);
        
        /// <summary>
        /// Remove items from this stack
        /// </summary>
        /// <param name="quantity">Quantity to remove</param>
        /// <returns>Quantity actually removed</returns>
        int RemoveItems(int quantity);
        
        /// <summary>
        /// Clear this stack completely
        /// </summary>
        void Clear();
        
        /// <summary>
        /// Clone this item stack
        /// </summary>
        /// <returns>New item stack with same contents</returns>
        IItemStack Clone();
    }

    /// <summary>
    /// Main inventory interface for managing items
    /// </summary>
    public interface IInventory
    {
        /// <summary>
        /// Event fired when inventory contents change
        /// </summary>
        event Action<IInventory> OnInventoryChanged;
        
        /// <summary>
        /// Event fired when an item is added
        /// </summary>
        event Action<IItem, int> OnItemAdded;
        
        /// <summary>
        /// Event fired when an item is removed
        /// </summary>
        event Action<IItem, int> OnItemRemoved;
        
        /// <summary>
        /// Event fired when inventory becomes full
        /// </summary>
        event Action<IInventory> OnInventoryFull;
        
        /// <summary>
        /// Total number of slots in this inventory
        /// </summary>
        int SlotCount { get; }
        
        /// <summary>
        /// Number of occupied slots
        /// </summary>
        int OccupiedSlots { get; }
        
        /// <summary>
        /// Number of empty slots
        /// </summary>
        int EmptySlots { get; }
        
        /// <summary>
        /// Whether the inventory is full
        /// </summary>
        bool IsFull { get; }
        
        /// <summary>
        /// Whether the inventory is empty
        /// </summary>
        bool IsEmpty { get; }
        
        /// <summary>
        /// Get item stack at specific slot
        /// </summary>
        /// <param name="slotIndex">Slot index</param>
        /// <returns>Item stack at slot, or null if empty</returns>
        IItemStack GetSlot(int slotIndex);
        
        /// <summary>
        /// Set item stack at specific slot
        /// </summary>
        /// <param name="slotIndex">Slot index</param>
        /// <param name="itemStack">Item stack to set</param>
        void SetSlot(int slotIndex, IItemStack itemStack);
        
        /// <summary>
        /// Get all item stacks in inventory
        /// </summary>
        /// <returns>Array of all item stacks</returns>
        IItemStack[] GetAllSlots();
        
        /// <summary>
        /// Add items to inventory (auto-stack and find slots) with detailed results
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <param name="quantity">Quantity to add</param>
        /// <returns>Detailed result of the add operation</returns>
        AddItemResult AddItem(IItem item, int quantity = 1);
        
        /// <summary>
        /// Remove items from inventory with detailed results
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <param name="quantity">Quantity to remove</param>
        /// <returns>Detailed result of the remove operation</returns>
        AddItemResult RemoveItem(IItem item, int quantity = 1);
        
        /// <summary>
        /// Remove items by item ID with detailed results
        /// </summary>
        /// <param name="itemId">Item ID to remove</param>
        /// <param name="quantity">Quantity to remove</param>
        /// <returns>Detailed result of the remove operation</returns>
        AddItemResult RemoveItem(string itemId, int quantity = 1);
        
        /// <summary>
        /// Check if inventory contains specific item
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <param name="quantity">Required quantity</param>
        /// <returns>True if inventory contains enough of the item</returns>
        bool ContainsItem(IItem item, int quantity = 1);
        
        /// <summary>
        /// Check if inventory contains item by ID
        /// </summary>
        /// <param name="itemId">Item ID to check</param>
        /// <param name="quantity">Required quantity</param>
        /// <returns>True if inventory contains enough of the item</returns>
        bool ContainsItem(string itemId, int quantity = 1);
        
        /// <summary>
        /// Get total quantity of specific item
        /// </summary>
        /// <param name="item">Item to count</param>
        /// <returns>Total quantity of item in inventory</returns>
        int GetItemCountByTypeID(IItem item);
        
        /// <summary>
        /// Get total quantity of item by ID
        /// </summary>
        /// <param name="itemId">Item ID to count</param>
        /// <returns>Total quantity of item in inventory</returns>
        int GetItemCountByTypeID(string itemId);
        
        /// <summary>
        /// Check if item can be added to inventory
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <param name="quantity">Quantity to add</param>
        /// <returns>True if item can be added</returns>
        bool CanAddItem(IItem item, int quantity = 1);
        
        /// <summary>
        /// Find first slot containing specific item
        /// </summary>
        /// <param name="item">Item to find</param>
        /// <returns>Slot index, or -1 if not found</returns>
        int FindItemSlot(IItem item);
        
        /// <summary>
        /// Find first slot containing item by ID
        /// </summary>
        /// <param name="itemId">Item ID to find</param>
        /// <returns>Slot index, or -1 if not found</returns>
        int FindItemSlot(string itemId);
        
        /// <summary>
        /// Find all slots containing specific item
        /// </summary>
        /// <param name="item">Item to find</param>
        /// <returns>Array of slot indices</returns>
        int[] FindAllItemSlots(IItem item);
        
        /// <summary>
        /// Find first empty slot
        /// </summary>
        /// <returns>Slot index, or -1 if no empty slots</returns>
        int FindEmptySlot();
        
        /// <summary>
        /// Find all empty slots
        /// </summary>
        /// <returns>Array of empty slot indices</returns>
        int[] FindAllEmptySlots();
        
        /// <summary>
        /// Swap items between two slots
        /// </summary>
        /// <param name="slotA">First slot index</param>
        /// <param name="slotB">Second slot index</param>
        void SwapSlots(int slotA, int slotB);
        
        /// <summary>
        /// Move items from one slot to another
        /// </summary>
        /// <param name="fromSlot">Source slot index</param>
        /// <param name="toSlot">Destination slot index</param>
        /// <param name="quantity">Quantity to move (0 = all)</param>
        /// <returns>Quantity actually moved</returns>
        int MoveItems(int fromSlot, int toSlot, int quantity = 0);
        
        /// <summary>
        /// Clear all items from inventory
        /// </summary>
        void Clear();
        
        /// <summary>
        /// Clear specific slot
        /// </summary>
        /// <param name="slotIndex">Slot to clear</param>
        void ClearSlot(int slotIndex);
        
        /// <summary>
        /// Get all unique items in inventory
        /// </summary>
        /// <returns>List of unique items</returns>
        List<IItem> GetUniqueItems();
        
        /// <summary>
        /// Get items by rarity level
        /// </summary>
        /// <param name="rarity">Rarity level to filter by</param>
        /// <returns>List of items with specified rarity</returns>
        List<IItemStack> GetItemsByRarity(int rarity);
        
        /// <summary>
        /// Sort inventory by specified criteria
        /// </summary>
        /// <param name="sortType">Type of sorting to apply</param>
        void SortInventory(InventorySortType sortType);
        
        /// <summary>
        /// Compact inventory by merging stackable items
        /// </summary>
        void CompactInventory();
        
        /// <summary>
        /// Get inventory data for serialization
        /// </summary>
        /// <returns>Serializable inventory data</returns>
        InventoryData GetInventoryData();
        
        /// <summary>
        /// Load inventory from serialized data
        /// </summary>
        /// <param name="data">Inventory data to load</param>
        void LoadInventoryData(InventoryData data);
    }

    /// <summary>
    /// Inventory sorting options
    /// </summary>
    public enum InventorySortType
    {
        None,
        Name,
        Rarity,
        MaxStack,
        Quantity
    }

    /// <summary>
    /// Serializable inventory data structure
    /// </summary>
    [Serializable]
    public class InventoryData
    {
        public ItemStackData[] slots;
        
        [Serializable]
        public class ItemStackData
        {
            public string itemId;
            public int quantity;
        }
    }
}