using System;
using System.Collections.Generic;
using System.Linq;
using Seyren.System.Items;
using Seyren.System.Inventory;
using UnityEngine;

namespace Seyren.System.Inventories
{
    /// <summary>
    /// Grid-based inventory implementation supporting variable-sized items (Diablo-style)
    /// </summary>
    public class GridBasedInventory : IInventory
    {
        #region Events
        public event Action<IInventory> OnInventoryChanged;
        public event Action<IItem, int> OnItemAdded;
        public event Action<IItem, int> OnItemRemoved;
        public event Action<IInventory> OnInventoryFull;
        #endregion

        #region Private Fields
        private struct Cell
        {
            public int Row;
            public int Column;
            public Cell(int row, int column)
            {
                this.Row = row;
                this.Column = column;
            }
        }

        private readonly int rows;
        private readonly int columns;
        private readonly IItem[,] cells;
        private readonly Dictionary<IItem, Cell> itemPositions;
        private readonly Dictionary<string, IItem> itemIdMap;
        #endregion

        #region Properties
        public int SlotCount => rows * columns;
        public int OccupiedSlots => itemPositions.Keys.Sum(item => item.Width * item.Height);
        public int EmptySlots => SlotCount - OccupiedSlots;
        public bool IsFull => EmptySlots == 0;
        public bool IsEmpty => itemPositions.Count == 0;
        public int Rows => rows;
        public int Columns => columns;
        #endregion

        #region Constructor
        public GridBasedInventory(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            this.cells = new IItem[rows, columns];
            this.itemPositions = new Dictionary<IItem, Cell>();
            this.itemIdMap = new Dictionary<string, IItem>();
        }
        #endregion

        #region Grid-Specific Methods
        /// <summary>
        /// Insert item at first available position
        /// </summary>
        public bool InsertItem(IItem item)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (CanInsertAt(item, row, col))
                    {
                        return InsertItemAt(item, row, col);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Insert item at specific grid position
        /// </summary>
        public bool InsertItemAt(IItem item, int row, int column)
        {
            if (!CanInsertAt(item, row, column)) return false;

            // Place item in grid
            for (int r = 0; r < item.Height; r++)
            {
                for (int c = 0; c < item.Width; c++)
                {
                    cells[row + r, column + c] = item;
                }
            }

            // Track item position
            itemPositions[item] = new Cell(row, column);
            itemIdMap[item.ID] = item;

            OnItemAdded?.Invoke(item, item.Count > 0 ? item.Count : 1);
            OnInventoryChanged?.Invoke(this);

            if (IsFull)
                OnInventoryFull?.Invoke(this);

            return true;
        }

        /// <summary>
        /// Check if item can be placed at specific position
        /// </summary>
        public bool CanInsertAt(IItem item, int row, int column)
        {
            if (item == null) return false;
            if (row < 0 || column < 0) return false;
            if (row + item.Height > rows || column + item.Width > columns) return false;

            // Check if all required cells are empty
            for (int r = 0; r < item.Height; r++)
            {
                for (int c = 0; c < item.Width; c++)
                {
                    if (cells[row + r, column + c] != null)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Remove item at specific grid position
        /// </summary>
        public bool RemoveItemAt(int row, int column)
        {
            if (row < 0 || row >= rows || column < 0 || column >= columns)
                return false;

            IItem item = cells[row, column];
            if (item == null) return false;

            return RemoveItemCompletely(item);
        }

        /// <summary>
        /// Get item at specific grid position
        /// </summary>
        public IItem GetItemAt(int row, int column)
        {
            if (row < 0 || row >= rows || column < 0 || column >= columns)
                return null;

            return cells[row, column];
        }

        /// <summary>
        /// Get position of item in grid
        /// </summary>
        public (int row, int column) GetItemPosition(IItem item)
        {
            if (item == null || !itemPositions.ContainsKey(item))
                return (-1, -1);

            Cell pos = itemPositions[item];
            return (pos.Row, pos.Column);
        }
        #endregion

        #region IInventory Implementation
        public IItem GetSlot(int slotIndex)
        {
            int row = slotIndex / columns;
            int col = slotIndex % columns;
            return GetItemAt(row, col);
        }

        public void SetSlot(int slotIndex, IItem item)
        {
            int row = slotIndex / columns;
            int col = slotIndex % columns;

            // Clear current slot first
            RemoveItemAt(row, col);

            // Insert new item if provided
            if (item != null)
            {
                InsertItemAt(item, row, col);
            }
        }

        public IItem[] GetAllSlots()
        {
            var slots = new IItem[SlotCount];
            for (int i = 0; i < SlotCount; i++)
            {
                slots[i] = GetSlot(i);
            }
            return slots;
        }

        public AddItemResult AddItem(IItem item)
        {
            if (item == null || item.Count <= 0)
            {
                return new AddItemResult(new List<IItem>(), 0, ItemKind.Unique);
            }
            ItemKind kind = item.MaxStack == 1 ? ItemKind.Unique : ItemKind.Stackable;
            if (InsertItem(item)) {
                return new AddItemResult(new List<IItem> { item }, 1, kind);
            }
            return new AddItemResult(new List<IItem> { item }, 1, kind);
        }

        public AddItemResult RemoveItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                return new AddItemResult(new List<IItem>(), 0, ItemKind.Unique);
            }
            
            IItem item = GetItemByID(itemId);
            if (item == null)
            {
                return new AddItemResult(new List<IItem>(), 0, ItemKind.Unique);
            }

            int totalRemoved = 0;
            var removedItems = new List<IItem>();
            ItemKind itemKind = ItemKind.Unique;
            RemoveItemCompletely(item);

            return new AddItemResult(removedItems, totalRemoved, itemKind);
        }


        public bool ContainsItem(string itemId)
        {
            return ContainsItemID(itemId);
        }

        public bool ContainsItem(string itemId, int quantity)
        {
            return GetItemCountByItemID(itemId) >= quantity;
        }

        public bool ContainsItemType(string itemTypeId)
        {
            return itemPositions.Keys.Any(item => item.TypeId == itemTypeId);
        }

        public bool ContainsItemID(string itemId)
        {
            return !string.IsNullOrEmpty(itemId) && itemIdMap.ContainsKey(itemId);
        }

        public IItem GetItemByID(string itemId)
        {
            if (string.IsNullOrEmpty(itemId) || !itemIdMap.ContainsKey(itemId)) {
                return null;
            }
                
            return itemIdMap[itemId];
        }

        public int GetItemCountByTypeID(IItem item)
        {
            return item != null ? GetItemCountByTypeID(item.TypeId) : 0;
        }

        public int GetItemCountByTypeID(string itemTypeId)
        {
            if (string.IsNullOrEmpty(itemTypeId)) return 0;

            return itemPositions.Keys.Where(item => item.TypeId == itemTypeId)
                               .Sum(item => item.Count);
        }

        public int GetItemCountByItemID(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return 0;

            return itemPositions.Keys.Where(item => item.ID == itemId)
                               .Sum(item => item.Count);
        }


        public bool CanAddItem(IItem item)
        {
            if (item == null || item.Count <= 0) return false;

            int quantity = item.Count;
            int remainingSpace = 0;
            foreach (var existingItem in itemPositions.Keys)
            {
                if (existingItem.ID == item.ID)
                {
                    remainingSpace += existingItem.MaxStack - existingItem.Count;
                }
            }

            // If existing items can accommodate all quantity
            if (remainingSpace >= quantity) return true;

            // Check if we can place new items for remaining quantity
            quantity -= remainingSpace;
            int slotsNeeded = Mathf.CeilToInt((float)quantity / item.MaxStack);

            // Estimate available slots (this is approximate for grid-based inventory)
            return slotsNeeded <= GetAvailableSlots(item);
        }

        public int FindItemSlot(IItem item)
        {
            if (item == null || !itemPositions.ContainsKey(item)) return -1;

            var pos = itemPositions[item];
            return pos.Row * columns + pos.Column;
        }

        public int FindItemSlot(string itemId)
        {
            if (string.IsNullOrEmpty(itemId) || !itemIdMap.ContainsKey(itemId))
                return -1;
                
            var item = itemIdMap[itemId];
            return FindItemSlot(item);
        }
        
        public int[] FindAllItemSlots(string itemTypeId)
        {
            if (string.IsNullOrEmpty(itemTypeId)) return new int[0];

            var slots = new List<int>();
            foreach (var existingItem in itemPositions.Keys)
            {
                if (existingItem.TypeId == itemTypeId)
                {
                    var pos = itemPositions[existingItem];
                    slots.Add(pos.Row * columns + pos.Column);
                }
            }
            return slots.ToArray();
        }

        public int FindEmptySlot()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (cells[row, col] == null)
                        return row * columns + col;
                }
            }
            return -1;
        }

        public int[] FindAllEmptySlots()
        {
            var emptySlots = new List<int>();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (cells[row, col] == null)
                        emptySlots.Add(row * columns + col);
                }
            }
            return emptySlots.ToArray();
        }

        public void SwapSlots(int slotA, int slotB)
        {
            var itemA = GetSlot(slotA);
            var itemB = GetSlot(slotB);

            SetSlot(slotA, null);
            SetSlot(slotB, null);

            if (itemB != null) SetSlot(slotA, itemB);
            if (itemA != null) SetSlot(slotB, itemA);
        }

        public int MoveItems(int fromSlot, int toSlot, int quantity = 0)
        {
            var fromItem = GetSlot(fromSlot);
            if (fromItem == null || fromItem.Count <= 0) return 0;

            var toItem = GetSlot(toSlot);
            int moveQuantity = quantity > 0 ? Math.Min(quantity, fromItem.Count) : fromItem.Count;

            if (toItem == null)
            {
                // Move to empty slot - create clone for partial moves
                if (moveQuantity < fromItem.Count)
                {
                    var newItem = CloneItem(fromItem);
                    newItem.Count = moveQuantity;
                    SetSlot(toSlot, newItem);
                    fromItem.Count -= moveQuantity;
                }
                else
                {
                    // Move entire item
                    SetSlot(toSlot, fromItem);
                    SetSlot(fromSlot, null);
                }
                return moveQuantity;
            }
            else if (toItem.ID == fromItem.ID && toItem.Count < toItem.MaxStack)
            {
                // Stack items
                int canStack = Math.Min(moveQuantity, toItem.MaxStack - toItem.Count);
                toItem.Count += canStack;
                fromItem.Count -= canStack;

                if (fromItem.Count <= 0)
                    SetSlot(fromSlot, null);

                return canStack;
            }

            return 0; // Cannot move incompatible items
        }

        public void Clear()
        {
            Array.Clear(cells, 0, cells.Length);
            itemPositions.Clear();
            itemIdMap.Clear();
            OnInventoryChanged?.Invoke(this);
        }

        public void ClearSlot(int slotIndex)
        {
            int row = slotIndex / columns;
            int col = slotIndex % columns;
            RemoveItemAt(row, col);
        }

        public List<IItem> GetUniqueItems()
        {
            return itemPositions.Keys.GroupBy(item => item.ID)
                               .Select(group => group.First())
                               .ToList();
        }

        public List<IItem> GetItemsByRarity(int rarity)
        {
            return itemPositions.Keys.Where(item => item.Rarity == rarity).ToList();
        }

        public void SortInventory(InventorySortType sortType)
        {
            // For grid-based inventory, sorting is more complex
            // We'll collect all items, clear the inventory, and re-add them in sorted order
            var allItems = itemPositions.Keys.ToList();
            Clear();

            switch (sortType)
            {
                case InventorySortType.Name:
                    allItems = allItems.OrderBy(item => item.Name).ToList();
                    break;
                case InventorySortType.Rarity:
                    allItems = allItems.OrderByDescending(item => item.Rarity).ToList();
                    break;
                case InventorySortType.MaxStack:
                    allItems = allItems.OrderByDescending(item => item.MaxStack).ToList();
                    break;
                case InventorySortType.Quantity:
                    allItems = allItems.OrderByDescending(item => item.Count).ToList();
                    break;
            }

            foreach (var item in allItems)
            {
                // Set the count and add the item
                var itemToAdd = CloneItem(item);
                itemToAdd.Count = item.Count;
                AddItem(itemToAdd);
            }
        }

        public void CompactInventory()
        {
            // Group items by ID and merge counts
            var itemGroups = itemPositions.Keys.GroupBy(item => item.ID)
                                           .ToDictionary(g => g.Key, g => g.Sum(item => item.Count));

            Clear();

            foreach (var group in itemGroups)
            {
                var sampleItem = itemPositions.Keys.FirstOrDefault(i => i.ID == group.Key);
                if (sampleItem != null)
                {
                    var itemToAdd = CloneItem(sampleItem);
                    itemToAdd.Count = group.Value;
                    AddItem(itemToAdd);
                }
            }
        }

        public InventoryData GetInventoryData()
        {
            var data = new InventoryData();
            var slotData = new List<InventoryData.ItemStackData>();

            foreach (var item in itemPositions.Keys)
            {
                slotData.Add(new InventoryData.ItemStackData
                {
                    itemId = item.ID,
                    quantity = item.Count
                });
            }

            data.slots = slotData.ToArray();
            return data;
        }

        public void LoadInventoryData(InventoryData data)
        {
            Clear();

            if (data?.slots != null)
            {
                foreach (var slot in data.slots)
                {
                    // Note: In a real implementation, you'd need a way to create IItem instances from IDs
                    // This might involve an ItemDatabase or ItemFactory
                    // For now, this is a placeholder
                    Debug.LogWarning($"Loading item {slot.itemId} with quantity {slot.quantity} requires item database implementation");
                }
            }
        }
        #endregion

        #region Private Helper Methods
        private bool RemoveItemCompletely(IItem item)
        {
            if (item == null || !itemPositions.ContainsKey(item)) return false;

            var pos = itemPositions[item];

            // Clear cells
            for (int r = 0; r < item.Height; r++)
            {
                for (int c = 0; c < item.Width; c++)
                {
                    cells[pos.Row + r, pos.Column + c] = null;
                }
            }

            // Remove tracking
            itemPositions.Remove(item);
            itemIdMap.Remove(item.ID);

            OnItemRemoved?.Invoke(item, item.Count);
            OnInventoryChanged?.Invoke(this);

            return true;
        }

        private int GetAvailableSlots(IItem item)
        {
            int count = 0;
            for (int row = 0; row <= rows - item.Height; row++)
            {
                for (int col = 0; col <= columns - item.Width; col++)
                {
                    if (CanInsertAt(item, row, col))
                        count++;
                }
            }
            return count;
        }

        private IItem CloneItem(IItem original)
        {
            // Note: In a real implementation, you'd need proper item cloning
            // This is a simplified placeholder
            return original.Clone();
        }
        #endregion


    }
}