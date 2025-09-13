using NUnit.Framework;
using Seyren.System.Inventories;
using Seyren.System.Inventory;
using Seyren.System.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Seyren.Tests.System
{
    [TestFixture]
    public class GridBasedInventoryTests
    {
        private GridBasedInventory inventory;
        private MockItem smallItem;
        private MockItem largeItem;
        private MockItem stackableItem;
        private MockItem nonStackableItem;

        [SetUp]
        public void Setup()
        {
            inventory = new GridBasedInventory(10, 8); // 10x8 grid
            
            // Create test items
            smallItem = new MockItem("small_item_001", "Small Item", 1, 1, 1, 1, "small_item"); // 1x1, non-stackable
            largeItem = new MockItem("large_item_001", "Large Item", 2, 3, 1, 1, "large_item"); // 2x3, non-stackable  
            stackableItem = new MockItem("stackable_item_001", "Stackable Item", 1, 1, 50, 10, "stackable_item"); // 1x1, max stack 50
            nonStackableItem = new MockItem("non_stackable_001", "Non-Stackable", 1, 1, 1, 1, "non_stackable"); // 1x1, non-stackable
        }

        [TearDown]
        public void TearDown()
        {
            inventory.Clear();
        }

        #region Basic Grid Operations Tests

        [Test]
        public void Constructor_ValidDimensions_CreatesInventory()
        {
            // Arrange & Act
            var testInventory = new GridBasedInventory(5, 6);

            // Assert
            Assert.AreEqual(5, testInventory.Rows);
            Assert.AreEqual(6, testInventory.Columns);
            Assert.AreEqual(30, testInventory.SlotCount);
            Assert.IsTrue(testInventory.IsEmpty);
            Assert.IsFalse(testInventory.IsFull);
        }

        [Test]
        public void InsertItem_ValidItem_InsertsSuccessfully()
        {
            // Act
            bool result = inventory.InsertItem(smallItem);

            // Assert
            Assert.IsTrue(result);
            Assert.IsFalse(inventory.IsEmpty);
            Assert.AreEqual(1, inventory.OccupiedSlots);
            Assert.AreEqual(79, inventory.EmptySlots);
            Assert.AreEqual(smallItem, inventory.GetItemAt(0, 0));
        }

        [Test]
        public void InsertItemAt_ValidPosition_InsertsAtSpecificLocation()
        {
            // Act
            bool result = inventory.InsertItemAt(smallItem, 3, 4);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(smallItem, inventory.GetItemAt(3, 4));
            var position = inventory.GetItemPosition(smallItem);
            Assert.AreEqual((3, 4), position);
        }

        [Test]
        public void InsertItemAt_InvalidPosition_ReturnsFalse()
        {
            // Act & Assert
            Assert.IsFalse(inventory.InsertItemAt(smallItem, -1, 0));
            Assert.IsFalse(inventory.InsertItemAt(smallItem, 0, -1));
            Assert.IsFalse(inventory.InsertItemAt(smallItem, 10, 0));
            Assert.IsFalse(inventory.InsertItemAt(smallItem, 0, 8));
        }

        [Test]
        public void InsertItemAt_LargeItem_OccupiesMultipleCells()
        {
            // Act
            bool result = inventory.InsertItemAt(largeItem, 1, 1);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(6, inventory.OccupiedSlots); // 2x3 = 6 cells
            
            // Check all cells are occupied by the large item
            for (int r = 0; r < largeItem.Height; r++)
            {
                for (int c = 0; c < largeItem.Width; c++)
                {
                    Assert.AreEqual(largeItem, inventory.GetItemAt(1 + r, 1 + c));
                }
            }
        }

        [Test]
        public void CanInsertAt_ValidPosition_ReturnsTrue()
        {
            // Act & Assert
            Assert.IsTrue(inventory.CanInsertAt(smallItem, 0, 0));
            Assert.IsTrue(inventory.CanInsertAt(largeItem, 0, 0));
            Assert.IsTrue(inventory.CanInsertAt(largeItem, 7, 5)); // Should fit at edge
        }

        [Test]
        public void CanInsertAt_InvalidPosition_ReturnsFalse()
        {
            // Act & Assert
            Assert.IsFalse(inventory.CanInsertAt(largeItem, 9, 6)); // Would exceed grid
            Assert.IsFalse(inventory.CanInsertAt(smallItem, -1, 0));
            Assert.IsFalse(inventory.CanInsertAt(null, 0, 0));
        }

        [Test]
        public void CanInsertAt_OverlappingItem_ReturnsFalse()
        {
            // Arrange
            inventory.InsertItemAt(smallItem, 2, 2);

            // Act & Assert
            Assert.IsFalse(inventory.CanInsertAt(largeItem, 1, 1)); // Would overlap with smallItem
            Assert.IsTrue(inventory.CanInsertAt(largeItem, 3, 3)); // Should not overlap
        }

        [Test]
        public void RemoveItemAt_ExistingItem_RemovesSuccessfully()
        {
            // Arrange
            inventory.InsertItemAt(smallItem, 2, 3);

            // Act
            bool result = inventory.RemoveItemAt(2, 3);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(inventory.GetItemAt(2, 3));
            Assert.IsTrue(inventory.IsEmpty);
        }

        [Test]
        public void RemoveItemAt_LargeItem_ClearsAllCells()
        {
            // Arrange
            inventory.InsertItemAt(largeItem, 1, 1);

            // Act
            bool result = inventory.RemoveItemAt(1, 1);

            // Assert
            Assert.IsTrue(result);
            
            // Check all cells are cleared
            for (int r = 0; r < largeItem.Height; r++)
            {
                for (int c = 0; c < largeItem.Width; c++)
                {
                    Assert.IsNull(inventory.GetItemAt(1 + r, 1 + c));
                }
            }
        }

        [Test]
        public void RemoveItemAt_EmptyCell_ReturnsFalse()
        {
            // Act & Assert
            Assert.IsFalse(inventory.RemoveItemAt(0, 0));
            Assert.IsFalse(inventory.RemoveItemAt(-1, 0));
            Assert.IsFalse(inventory.RemoveItemAt(10, 8));
        }

        [Test]
        public void GetItemAt_InvalidPosition_ReturnsNull()
        {
            // Act & Assert
            Assert.IsNull(inventory.GetItemAt(-1, 0));
            Assert.IsNull(inventory.GetItemAt(0, -1));
            Assert.IsNull(inventory.GetItemAt(10, 0));
            Assert.IsNull(inventory.GetItemAt(0, 8));
        }

        [Test]
        public void GetItemPosition_ExistingItem_ReturnsCorrectPosition()
        {
            // Arrange
            inventory.InsertItemAt(smallItem, 5, 3);

            // Act
            var position = inventory.GetItemPosition(smallItem);

            // Assert
            Assert.AreEqual((5, 3), position);
        }

        [Test]
        public void GetItemPosition_NonExistentItem_ReturnsInvalidPosition()
        {
            // Act
            var position = inventory.GetItemPosition(smallItem);

            // Assert
            Assert.AreEqual((-1, -1), position);
        }

        #endregion

        #region IInventory Interface Tests

        [Test]
        public void AddItem_SingleItem_AddsSuccessfully()
        {
            // Act
            int added = inventory.AddItem(smallItem, 1).TotalAdded;

            // Assert
            Assert.AreEqual(1, added);
            Assert.AreEqual(1, inventory.GetItemCountByTypeID(smallItem.TypeId));
            Assert.IsFalse(inventory.IsEmpty);
        }

        [Test]
        public void AddItem_StackableItems_StacksCorrectly()
        {
            // Act
            int firstAdd = inventory.AddItem(stackableItem, 10).TotalAdded;
            int secondAdd = inventory.AddItem(stackableItem, 15).TotalAdded;

            // Assert
            Assert.AreEqual(10, firstAdd);
            Assert.AreEqual(15, secondAdd);
            Assert.AreEqual(25, inventory.GetItemCountByTypeID(stackableItem.TypeId));
            
            // Should only occupy one slot since they stack
            Assert.AreEqual(1, inventory.OccupiedSlots);
        }

        [Test]
        public void AddItem_StackableItemsExceedingMaxStack_CreatesMultipleStacks()
        {
            int numberOfStacks = 75;
            // Act - Try to add more than max stack size
            int added = inventory.AddItem(stackableItem, numberOfStacks).TotalAdded; // Max stack is 50
            int currentCount = inventory.GetItemCountByTypeID(stackableItem.TypeId);
            // Assert
            Assert.AreEqual(numberOfStacks, added);
            Assert.AreEqual(numberOfStacks, currentCount);
            
            // Should create 2 stacks: 50 + 25
            Assert.AreEqual(2, inventory.OccupiedSlots);
        }

        [Test]
        public void AddItem_NoSpace_ReturnsZero()
        {
            // inventory.Clear();

            int w = inventory.Columns;
            int h = inventory.Rows;
            // Arrange - Fill inventory with large items
            IItem item = new MockItem("filler", "Filler", w - 1, h - 1, 1, 1);
            inventory.InsertItemAt(item, 0, 0); // Fill most of the grid
            // for (int r = 0; r <= inventory.Rows; r += h) // Fill rows with 2x3 items
            // {
            //     for (int c = 0; c <= inventory.Columns; c += w)
            //     {
            //         IItem item = new MockItem($"item_{r}_{c}", "Test Item", w, h, 1, 1);
            //         inventory.AddItem(item);
            //         // Assert.IsTrue(success);
            //     }
            // }

            // Act
            int added = inventory.AddItem(largeItem, 1).TotalAdded;

            // Assert
            Assert.AreEqual(0, added);
        }

        [Test]
        public void RemoveItem_ExistingItem_RemovesCorrectQuantity()
        {
            // Arrange
            inventory.AddItem(stackableItem, 30);

            // Act
            AddItemResult result = inventory.RemoveItem(stackableItem, 10);

            // Assert
            Assert.AreEqual(10, result.TotalAdded);
            Assert.AreEqual(20, inventory.GetItemCountByTypeID(stackableItem.TypeId));
            Assert.AreEqual(ItemKind.Stackable, result.Kind);
        }

        [Test]
        public void RemoveItem_ByItemId_RemovesCorrectQuantity()
        {
            // Arrange
            inventory.AddItem(stackableItem, 20);

            // Act
            AddItemResult result = inventory.RemoveItem(stackableItem.ID, 5);
            int currentCount = inventory.GetItemCountByTypeID(stackableItem.TypeId);

            // Assert
            Assert.AreEqual(5, result.TotalAdded);
            Assert.AreEqual(15, currentCount);
            Assert.AreEqual(ItemKind.Stackable, result.Kind);
        }

        [Test]
        public void RemoveItem_MoreThanAvailable_RemovesAllAvailable()
        {
            // Arrange
            inventory.AddItem(stackableItem, 15);

            // Act
            var result = inventory.RemoveItem(stackableItem, 20);

            // Assert
            Assert.AreEqual(15, result.TotalAdded);
            Assert.AreEqual(0, inventory.GetItemCountByTypeID(stackableItem.TypeId));
            Assert.IsTrue(inventory.IsEmpty);
            Assert.AreEqual(ItemKind.Stackable, result.Kind);
        }

        [Test]
        public void ContainsItem_ExistingItem_ReturnsTrue()
        {
            // Arrange
            inventory.AddItem(smallItem, 1);

            // Act & Assert
            Assert.IsTrue(inventory.ContainsItem(smallItem, 1));
            Assert.IsTrue(inventory.ContainsItem(smallItem.ID, 1));
            Assert.IsFalse(inventory.ContainsItem(smallItem, 2));
        }

        [Test]
        public void ContainsItem_NonExistentItem_ReturnsFalse()
        {
            // Act & Assert
            Assert.IsFalse(inventory.ContainsItem(smallItem, 1));
            Assert.IsFalse(inventory.ContainsItem("non_existent", 1));
        }

        [Test]
        public void CanAddItem_HasSpace_ReturnsTrue()
        {
            // Act & Assert
            Assert.IsTrue(inventory.CanAddItem(smallItem, 1));
            Assert.IsTrue(inventory.CanAddItem(stackableItem, 100));
        }

        [Test]
        public void CanAddItem_NoSpace_ReturnsFalse()
        {
            // Arrange - Fill inventory completely
            for (int r = 0; r < 10; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    var item = new MockItem($"filler_{r}_{c}", "Filler", 1, 1, 1, 1);
                    inventory.InsertItemAt(item, r, c);
                }
            }

            // Act & Assert
            Assert.IsFalse(inventory.CanAddItem(smallItem, 1));
        }

        [Test]
        public void FindItemSlot_ExistingItem_ReturnsCorrectSlot()
        {
            // Arrange
            inventory.InsertItemAt(smallItem, 2, 3); // Row 2, Column 3

            // Act
            int slot = inventory.FindItemSlot(smallItem);

            // Assert
            Assert.AreEqual(2 * 8 + 3, slot); // row * columns + column = 19
        }

        [Test]
        public void FindItemSlot_ByItemId_ReturnsCorrectSlot()
        {
            // Arrange
            inventory.InsertItemAt(smallItem, 1, 5);

            // Act
            int slot = inventory.FindItemSlot(smallItem.ID);

            // Assert
            Assert.AreEqual(1 * 8 + 5, slot); // 13
        }

        [Test]
        public void FindItemSlot_NonExistentItem_ReturnsMinusOne()
        {
            // Act & Assert
            Assert.AreEqual(-1, inventory.FindItemSlot(smallItem));
            Assert.AreEqual(-1, inventory.FindItemSlot("non_existent"));
        }

        [Test]
        public void FindAllItemSlots_MultipleStacks_ReturnsAllSlots()
        {
            // Arrange
            inventory.AddItem(stackableItem, 75); // Creates 2 stacks

            // Act
            int[] slots = inventory.FindAllItemSlots(stackableItem);

            // Assert
            Assert.AreEqual(2, slots.Length);
            Assert.Contains(0, slots); // First stack at (0,0)
            Assert.Contains(1, slots); // Second stack at (0,1)
        }

        [Test]
        public void FindEmptySlot_HasEmptySlots_ReturnsFirstEmpty()
        {
            // Arrange
            inventory.InsertItemAt(smallItem, 0, 0);

            // Act
            int emptySlot = inventory.FindEmptySlot();

            // Assert
            Assert.AreEqual(1, emptySlot); // Second slot (0,1)
        }

        [Test]
        public void FindEmptySlot_FullInventory_ReturnsMinusOne()
        {
            // Arrange - Fill inventory
            for (int r = 0; r < 10; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    var item = new MockItem($"filler_{r}_{c}", "Filler", 1, 1, 1, 1);
                    inventory.InsertItemAt(item, r, c);
                }
            }

            // Act
            int emptySlot = inventory.FindEmptySlot();

            // Assert
            Assert.AreEqual(-1, emptySlot);
        }

        [Test]
        public void GetSlot_ValidIndex_ReturnsItemStack()
        {
            // Arrange
            inventory.InsertItemAt(smallItem, 1, 2);

            // Act
            var slot = inventory.GetSlot(1 * 8 + 2); // Slot 10

            // Assert
            Assert.IsNotNull(slot);
            Assert.AreEqual(smallItem, slot.Item);
        }

        [Test]
        public void GetSlot_EmptySlot_ReturnsNull()
        {
            // Act
            var slot = inventory.GetSlot(0);

            // Assert
            Assert.IsNull(slot);
        }

        [Test]
        public void SetSlot_ValidSlot_SetsItemStack()
        {
            // Arrange
            var mockStack = new MockItemStack(smallItem, 1);

            // Act
            inventory.SetSlot(5, mockStack);

            // Assert
            var retrievedStack = inventory.GetSlot(5);
            Assert.IsNotNull(retrievedStack);
            Assert.AreEqual(smallItem, retrievedStack.Item);
        }

        [Test]
        public void Clear_InventoryWithItems_ClearsAll()
        {
            // Arrange
            inventory.AddItem(smallItem, 1);
            inventory.AddItem(stackableItem, 10);

            // Act
            inventory.Clear();

            // Assert
            Assert.IsTrue(inventory.IsEmpty);
            Assert.AreEqual(0, inventory.GetItemCountByTypeID(smallItem.TypeId));
            Assert.AreEqual(0, inventory.GetItemCountByTypeID(stackableItem.TypeId));
        }

        [Test]
        public void SortInventory_ByRarity_SortsCorrectly()
        {
            // Arrange
            var commonItem = new MockItem("common", "Common", 1, 1, 1, 1) { Rarity = 1 };
            var rareItem = new MockItem("rare", "Rare", 1, 1, 1, 1) { Rarity = 5 };
            var epicItem = new MockItem("epic", "Epic", 1, 1, 1, 1) { Rarity = 10 };
            
            inventory.AddItem(commonItem, 1);
            inventory.AddItem(rareItem, 1);
            inventory.AddItem(epicItem, 1);

            // Act
            inventory.SortInventory(InventorySortType.Rarity);

            // Assert - Items should be sorted by rarity (highest first)
            Assert.AreEqual(epicItem, inventory.GetItemAt(0, 0));
            Assert.AreEqual(rareItem, inventory.GetItemAt(0, 1));
            Assert.AreEqual(commonItem, inventory.GetItemAt(0, 2));
        }

        #endregion

        #region Event Tests

        [Test]
        public void AddItem_TriggersOnItemAdded()
        {
            // Arrange
            IItem addedItem = null;
            int addedQuantity = 0;
            inventory.OnItemAdded += (item, quantity) => { addedItem = item; addedQuantity = quantity; };

            // Act
            inventory.AddItem(smallItem, 1);

            // Assert
            Assert.AreEqual(smallItem, addedItem);
            Assert.AreEqual(1, addedQuantity);
        }

        [Test]
        public void RemoveItem_TriggersOnItemRemoved()
        {
            // Arrange
            inventory.AddItem(stackableItem, 10);
            IItem removedItem = null;
            int removedQuantity = 0;
            inventory.OnItemRemoved += (item, quantity) => { removedItem = item; removedQuantity = quantity; };

            // Act
            inventory.RemoveItem(stackableItem, 5);

            // Assert
            Assert.AreEqual(stackableItem, removedItem);
            Assert.AreEqual(5, removedQuantity);
        }

        [Test]
        public void AddItem_TriggersOnInventoryChanged()
        {
            // Arrange
            bool inventoryChanged = false;
            inventory.OnInventoryChanged += (inv) => inventoryChanged = true;

            // Act
            inventory.AddItem(smallItem, 1);

            // Assert
            Assert.IsTrue(inventoryChanged);
        }

        [Test]
        public void InventoryFull_TriggersOnInventoryFull()
        {
            // Arrange
            bool inventoryFull = false;
            inventory.OnInventoryFull += (inv) => inventoryFull = true;

            int w = inventory.Columns;
            int h = inventory.Rows;
            
            // Fill inventory to capacity
            for (int r = 0; r < h; r++)
            {
                for (int c = 0; c < w; c++)
                {
                    var item = new MockItem($"filler_{r}_{c}", "Filler", 1, 1, 1, 1);
                    inventory.InsertItemAt(item, r, c);
                }
            }

            // Assert
            Assert.IsTrue(inventoryFull);
        }

        #endregion

        #region Edge Cases and Integration Tests

        [Test]
        public void MultipleOperations_ComplexScenario_WorksCorrectly()
        {
            // Complex scenario: Add items, remove some, check state
            
            // Add various items
            inventory.AddItem(stackableItem, 45);
            inventory.InsertItemAt(largeItem, 2, 2);
            inventory.AddItem(smallItem, 1);

            int initialCount = inventory.GetItemCountByTypeID(stackableItem.TypeId);


            // Verify initial state
            Assert.AreEqual(45, initialCount);
            Assert.AreEqual(largeItem, inventory.GetItemAt(2, 2));
            Assert.AreEqual(1, inventory.GetItemCountByTypeID(smallItem.TypeId));

            // Remove some items
            inventory.RemoveItem(stackableItem, 20);
            inventory.RemoveItemAt(2, 2);

            // Verify final state
            Assert.AreEqual(25, inventory.GetItemCountByTypeID(stackableItem.TypeId));
            Assert.IsNull(inventory.GetItemAt(2, 2));
            Assert.AreEqual(1, inventory.GetItemCountByTypeID(smallItem.TypeId));
        }

        [Test]
        public void StressTest_ManySmallItems_HandlesCorrectly()
        {
            // Add many small items to test performance and correctness
            int itemsToAdd = 80; // Fill the 10x8 grid
            int addedCount = 0;

            for (int i = 0; i < itemsToAdd; i++)
            {
                var item = new MockItem($"stress_item_{i}", $"Stress Item {i}", 1, 1, 1, 1);
                int added = inventory.AddItem(item, 1).TotalAdded;
                addedCount += added;
            }

            // Should add exactly 80 items (full grid)
            Assert.AreEqual(80, addedCount);
            Assert.IsTrue(inventory.IsFull);
            Assert.AreEqual(0, inventory.EmptySlots);
        }

        [Test]
        public void AddItem_SameTypeIdDifferentId_StacksCorrectly()
        {
            // Arrange - Create items with same TypeId but different IDs
            var item1 = new MockItem("potion_001", "Health Potion", 1, 1, 50, 10) { TypeId = "health_potion" };
            var item2 = new MockItem("potion_002", "Health Potion", 1, 1, 50, 15) { TypeId = "health_potion" };

            // Act
            int added1 = inventory.AddItem(item1, 10).TotalAdded;
            int added2 = inventory.AddItem(item2, 15).TotalAdded;

            // Assert - Should stack because they have same TypeId
            Assert.AreEqual(10, added1);
            Assert.AreEqual(15, added2);
            Assert.AreEqual(25, inventory.GetItemCountByTypeID("health_potion"));
            Assert.AreEqual(1, inventory.OccupiedSlots); // Should occupy only one slot
        }

        #endregion
    }

    #region Mock Classes

    /// <summary>
    /// Mock implementation of IItem for testing
    /// </summary>
    public class MockItem : IItem
    {
        public string ID { get; set; }
        public string TypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Rarity { get; set; }
        public int MaxStack { get; set; }

        public MockItem(string id, string name, int width, int height, int maxStack, int count, string typeId = null)
        {
            ID = id;
            TypeId = typeId ?? id; // Use ID as TypeId if not provided
            Name = name;
            Description = $"Mock item: {name}";
            Width = width;
            Height = height;
            MaxStack = maxStack;
            Count = count;
            Rarity = 1;
        }

        public void Use(UseItemData data)
        {
            // Mock implementation - do nothing
        }

        /// <summary>
        /// Creates a deep copy of this MockItem with a new unique ID
        /// </summary>
        /// <returns>A new IItem instance with the same properties but unique ID</returns>
        public IItem Clone()
        {
            return new MockItem(Guid.NewGuid().ToString(), Name, Width, Height, MaxStack, Count, TypeId)
            {
                Description = this.Description,
                Rarity = this.Rarity
            };
        }
    }

    /// <summary>
    /// Mock implementation of IItemStack for testing
    /// </summary>
    public class MockItemStack : IItemStack
    {
        public IItem Item { get; private set; }
        public int Quantity { get; set; }
        public bool IsEmpty => Quantity <= 0;
        public bool IsFull => Quantity >= Item.MaxStack;
        public int RemainingSpace => Item.MaxStack - Quantity;

        public MockItemStack(IItem item, int quantity)
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
            return new MockItemStack(Item, Quantity);
        }
    }

    #endregion
}