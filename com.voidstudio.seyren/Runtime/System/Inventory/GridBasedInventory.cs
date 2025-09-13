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
        private readonly Dictionary<IItem, ItemStack> itemStacks;
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
            this.itemStacks = new Dictionary<IItem, ItemStack>();
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

            // Track item position and create stack
            itemPositions[item] = new Cell(row, column);
            itemStacks[item] = new ItemStack(item, item.Count > 0 ? item.Count : 1);

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
        public IItemStack GetSlot(int slotIndex)
        {
            int row = slotIndex / columns;
            int col = slotIndex % columns;
            IItem item = GetItemAt(row, col);
            return item != null && itemStacks.ContainsKey(item) ? itemStacks[item] : null;
        }

        public void SetSlot(int slotIndex, IItemStack itemStack)
        {
            int row = slotIndex / columns;
            int col = slotIndex % columns;

            // Clear current slot first
            RemoveItemAt(row, col);

            // Insert new item if provided
            if (itemStack != null && itemStack.Item != null)
            {
                InsertItemAt(itemStack.Item, row, col);
                if (itemStacks.ContainsKey(itemStack.Item))
                {
                    itemStacks[itemStack.Item].Quantity = itemStack.Quantity;
                }
            }
        }

        public IItemStack[] GetAllSlots()
        {
            var slots = new IItemStack[SlotCount];
            for (int i = 0; i < SlotCount; i++)
            {
                slots[i] = GetSlot(i);
            }
            return slots;
        }

        public AddItemResult AddItem(IItem item, int quantity = 1)
        {
            if (item == null || quantity <= 0) 
                return new AddItemResult(new List<IItem>(), 0, ItemKind.Unique);

            if (item.MaxStack == 1) // Unique item
            {
                return AddUniqueItem(item);
            }

            return AddStackableItem(item, quantity);
        }

        private AddItemResult AddUniqueItem(IItem item)
        {
            var newItemsCreated = new List<IItem>();
            
            if (InsertItem(item))
            {
                newItemsCreated.Add(item);
                OnItemAdded?.Invoke(item, 1);
                OnInventoryChanged?.Invoke(this);
                return new AddItemResult(newItemsCreated, 1, ItemKind.Unique);
            }

            return new AddItemResult(newItemsCreated, 0, ItemKind.Unique);
        }


        private AddItemResult AddStackableItem(IItem item, int quantity)
        {
            if (item == null || quantity <= 0) 
                return new AddItemResult(new List<IItem>(), 0, ItemKind.Stackable);

            int totalAdded = 0;
            var newItemsCreated = new List<IItem>();
            
            // if item was not existing, create a new instance to avoid modifying original
            if (!ContainsItemType(item.TypeId))
            {
                int canAdd = Math.Min(quantity, item.MaxStack);
                item.Count = canAdd;

                if (!InsertItem(item))
                {
                    return new AddItemResult(newItemsCreated, 0, ItemKind.Stackable);
                }

                newItemsCreated.Add(item);
                totalAdded += canAdd;
                quantity -= canAdd;
            }
            else
            {
                // Try to stack with existing items first
                foreach (var existingItem in itemStacks.Keys.ToList())
                {
                    if (existingItem.TypeId == item.TypeId && itemStacks[existingItem].Quantity < existingItem.MaxStack)
                    {
                        var stack = itemStacks[existingItem];
                        int canAdd = Math.Min(quantity, existingItem.MaxStack - stack.Quantity);
                        stack.Quantity += canAdd;
                        quantity -= canAdd;
                        totalAdded += canAdd;

                        OnItemAdded?.Invoke(item, canAdd);
                        OnInventoryChanged?.Invoke(this);

                        if (quantity <= 0) 
                            return new AddItemResult(newItemsCreated, totalAdded, ItemKind.Stackable);
                    }
                }
            }

            // If we still have quantity to add, try to place new items
            while (quantity > 0)
            {
                IItem newItem = CloneItem(item);
                int stackSize = Math.Min(quantity, item.MaxStack);
                newItem.Count = stackSize;

                if (InsertItem(newItem))
                {
                    Debug.Log($"Inserted new item {newItem.Name} x{stackSize}");
                    newItemsCreated.Add(newItem);
                    totalAdded += stackSize;
                    quantity -= stackSize;
                }
                else
                {
                    break; // No more space
                }
            }

            return new AddItemResult(newItemsCreated, totalAdded, ItemKind.Stackable);
        }

        public AddItemResult RemoveItem(IItem item, int quantity = 1)
        {
            if (item == null || quantity <= 0) 
                return new AddItemResult(new List<IItem>(), 0, ItemKind.Unique);

            return RemoveItem(item.ID, quantity);
        }

        public AddItemResult RemoveItem(string itemId, int quantity = 1)
        {
            if (string.IsNullOrEmpty(itemId) || quantity <= 0) 
                return new AddItemResult(new List<IItem>(), 0, ItemKind.Unique);

            int totalRemoved = 0;
            var itemsToRemove = new List<IItem>();
            var removedItems = new List<IItem>();
            ItemKind itemKind = ItemKind.Unique;

            foreach (var kvp in itemStacks.ToList())
            {
                if (kvp.Key.ID == itemId && quantity > 0)
                {
                    var stack = kvp.Value;
                    int canRemove = Math.Min(quantity, stack.Quantity);
                    
                    // Determine item kind from the first matching item
                    if (totalRemoved == 0)
                    {
                        itemKind = kvp.Key.MaxStack > 1 ? ItemKind.Stackable : ItemKind.Unique;
                    }
                    
                    stack.Quantity -= canRemove;
                    totalRemoved += canRemove;
                    quantity -= canRemove;

                    if (stack.Quantity <= 0)
                    {
                        itemsToRemove.Add(kvp.Key);
                        removedItems.Add(kvp.Key);
                    }

                    OnItemRemoved?.Invoke(kvp.Key, canRemove);
                }
            }

            // Remove empty stacks
            foreach (var item in itemsToRemove)
            {
                RemoveItemCompletely(item);
            }

            if (totalRemoved > 0)
                OnInventoryChanged?.Invoke(this);

            return new AddItemResult(removedItems, totalRemoved, itemKind);
        }        public bool ContainsItem(IItem item, int quantity = 1)
        {
            return item != null && ContainsItem(item.ID, quantity);
        }

        public bool ContainsItem(string itemId, int quantity = 1)
        {
            return GetItemCountByItemID(itemId) >= quantity;
        }

        public bool ContainsItemType(string itemTypeId)
        {
            return itemStacks.Where(kvp => kvp.Key.TypeId == itemTypeId).Any();
        }

        public bool ContainsItemID(string itemId)
        {
            return itemStacks.Where(kvp => kvp.Key.ID == itemId).Any();
        }

        public int GetItemCountByTypeID(IItem item)
        {
            return item != null ? GetItemCountByTypeID(item.TypeId) : 0;
        }

        public int GetItemCountByTypeID(string itemTypeId)
        {
            if (string.IsNullOrEmpty(itemTypeId)) return 0;

            return itemStacks.Where(kvp => kvp.Key.TypeId == itemTypeId)
                           .Sum(kvp => kvp.Value.Quantity);
        }

        public int GetItemCountByItemID(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return 0;

            return itemStacks.Where(kvp => kvp.Key.ID == itemId)
                           .Sum(kvp => kvp.Value.Quantity);
        }


        public bool CanAddItem(IItem item, int quantity = 1)
        {
            if (item == null || quantity <= 0) return false;

            // Check existing stacks for available space
            int remainingSpace = 0;
            foreach (var kvp in itemStacks)
            {
                if (kvp.Key.ID == item.ID)
                {
                    remainingSpace += kvp.Key.MaxStack - kvp.Value.Quantity;
                }
            }

            // If existing stacks can accommodate all quantity
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
            var item = itemStacks.Keys.FirstOrDefault(i => i.ID == itemId);
            return FindItemSlot(item);
        }
        
        public int[] FindAllItemSlots(IItem item)
        {
            if (item == null) return new int[0];

            var slots = new List<int>();
            foreach (var kvp in itemStacks)
            {
                if (kvp.Key.TypeId == item.TypeId)
                {
                    var pos = itemPositions[kvp.Key];
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
            var fromStack = GetSlot(fromSlot);
            if (fromStack == null || fromStack.IsEmpty) return 0;

            var toStack = GetSlot(toSlot);
            int moveQuantity = quantity > 0 ? Math.Min(quantity, fromStack.Quantity) : fromStack.Quantity;

            if (toStack == null || toStack.IsEmpty)
            {
                // Move to empty slot
                var newStack = new ItemStack(fromStack.Item, moveQuantity);
                SetSlot(toSlot, newStack);
                fromStack.RemoveItems(moveQuantity);

                if (fromStack.IsEmpty)
                    SetSlot(fromSlot, null);

                return moveQuantity;
            }
            else if (toStack.Item.ID == fromStack.Item.ID)
            {
                // Stack items
                int actualMoved = toStack.AddItems(fromStack.Item, moveQuantity);
                fromStack.RemoveItems(actualMoved);

                if (fromStack.IsEmpty)
                    SetSlot(fromSlot, null);

                return actualMoved;
            }

            return 0; // Cannot move incompatible items
        }

        public void Clear()
        {
            Array.Clear(cells, 0, cells.Length);
            itemPositions.Clear();
            itemStacks.Clear();
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
            return itemStacks.Keys.GroupBy(item => item.ID)
                           .Select(group => group.First())
                           .ToList();
        }

        public List<IItemStack> GetItemsByRarity(int rarity)
        {
            return itemStacks.Where(kvp => kvp.Key.Rarity == rarity)
                           .Select(kvp => kvp.Value)
                           .ToList<IItemStack>();
        }

        public void SortInventory(InventorySortType sortType)
        {
            // For grid-based inventory, sorting is more complex
            // We'll collect all items, clear the inventory, and re-add them in sorted order
            var allItems = itemStacks.ToList();
            Clear();

            switch (sortType)
            {
                case InventorySortType.Name:
                    allItems = allItems.OrderBy(kvp => kvp.Key.Name).ToList();
                    break;
                case InventorySortType.Rarity:
                    allItems = allItems.OrderByDescending(kvp => kvp.Key.Rarity).ToList();
                    break;
                case InventorySortType.MaxStack:
                    allItems = allItems.OrderByDescending(kvp => kvp.Key.MaxStack).ToList();
                    break;
                case InventorySortType.Quantity:
                    allItems = allItems.OrderByDescending(kvp => kvp.Value.Quantity).ToList();
                    break;
            }

            foreach (var kvp in allItems)
            {
                AddItem(kvp.Key, kvp.Value.Quantity);
            }
        }

        public void CompactInventory()
        {
            // Group items by ID and merge stacks
            var itemGroups = itemStacks.GroupBy(kvp => kvp.Key.ID)
                                     .ToDictionary(g => g.Key, g => g.Sum(kvp => kvp.Value.Quantity));

            Clear();

            foreach (var group in itemGroups)
            {
                var sampleItem = itemStacks.Keys.FirstOrDefault(i => i.ID == group.Key);
                if (sampleItem != null)
                {
                    AddItem(sampleItem, group.Value);
                }
            }
        }

        public InventoryData GetInventoryData()
        {
            var data = new InventoryData();
            var slotData = new List<InventoryData.ItemStackData>();

            foreach (var kvp in itemStacks)
            {
                slotData.Add(new InventoryData.ItemStackData
                {
                    itemId = kvp.Key.ID,
                    quantity = kvp.Value.Quantity
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
            var stack = itemStacks[item];

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
            itemStacks.Remove(item);

            OnItemRemoved?.Invoke(item, stack.Quantity);
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

        #region Inner Classes
        private class ItemStack : IItemStack
        {
            public IItem Item { get; private set; }
            public int Quantity { get; set; }
            public bool IsEmpty => Quantity <= 0;
            public bool IsFull => Quantity >= Item.MaxStack;
            public int RemainingSpace => Item.MaxStack - Quantity;

            public ItemStack(IItem item, int quantity)
            {
                Item = item;
                Quantity = quantity;
            }

            public bool CanAddItems(IItem item, int quantity)
            {
                return item.ID == Item.ID && RemainingSpace >= quantity;
            }

            public int AddItems(IItem item, int quantity)
            {
                if (!CanAddItems(item, quantity)) return 0;

                int actualAdded = Math.Min(quantity, RemainingSpace);
                Quantity += actualAdded;
                return actualAdded;
            }

            public int RemoveItems(int quantity)
            {
                int actualRemoved = Math.Min(quantity, Quantity);
                Quantity -= actualRemoved;
                return actualRemoved;
            }

            public void Clear()
            {
                Quantity = 0;
            }

            public IItemStack Clone()
            {
                return new ItemStack(Item, Quantity);
            }
        }
        #endregion
    }
}