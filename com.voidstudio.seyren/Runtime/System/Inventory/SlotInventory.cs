using System;
using System.Collections.Generic;
using System.Linq;
using Seyren.System.Items;

namespace Seyren.System.Inventories
{
    /// <summary>
    /// Slot-based inventory where each slot holds exactly one item regardless of item size.
    /// Simpler than GridBasedInventory: no spatial layout, just a flat array of slots.
    /// </summary>
    public class SlotInventory : Seyren.System.Inventory.IInventory
    {
        private readonly IItem[] slots;

        public event Action<Seyren.System.Inventory.IInventory> OnInventoryChanged;
        public event Action<IItem, int> OnItemAdded;
        public event Action<IItem, int> OnItemRemoved;
        public event Action<Seyren.System.Inventory.IInventory> OnInventoryFull;

        public int SlotCount => slots.Length;
        public int OccupiedSlots => slots.Count(s => s != null);
        public int EmptySlots => slots.Count(s => s == null);
        public bool IsFull => EmptySlots == 0;
        public bool IsEmpty => OccupiedSlots == 0;

        public SlotInventory(int slotCount)
        {
            slots = new IItem[slotCount];
        }

        // ── Core slot access ──────────────────────────────────────────────

        public IItem GetSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= slots.Length) return null;
            return slots[slotIndex];
        }

        public void SetSlot(int slotIndex, IItem item)
        {
            if (slotIndex < 0 || slotIndex >= slots.Length) return;

            var old = slots[slotIndex];
            slots[slotIndex] = item;

            if (old != null)  OnItemRemoved?.Invoke(old, old.Count);
            if (item != null) OnItemAdded?.Invoke(item, item.Count);
            OnInventoryChanged?.Invoke(this);
        }

        public IItem[] GetAllSlots() => (IItem[])slots.Clone();

        // ── Add / Remove ──────────────────────────────────────────────────

        public Seyren.System.Inventory.AddItemResult AddItem(IItem item)
        {
            if (item == null)
                return new Seyren.System.Inventory.AddItemResult(new List<IItem>(), 0, Seyren.System.Inventory.ItemKind.Unique);

            int idx = FindEmptySlot();
            if (idx < 0)
            {
                OnInventoryFull?.Invoke(this);
                return new Seyren.System.Inventory.AddItemResult(new List<IItem>(), 0, Seyren.System.Inventory.ItemKind.Unique);
            }

            SetSlot(idx, item);
            return new Seyren.System.Inventory.AddItemResult(
                new List<IItem> { item }, 1, Seyren.System.Inventory.ItemKind.Unique);
        }

        public Seyren.System.Inventory.AddItemResult RemoveItem(string itemId)
        {
            var removed = new List<IItem>();
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null && slots[i].ID == itemId)
                {
                    removed.Add(slots[i]);
                    SetSlot(i, null);
                }
            }
            return new Seyren.System.Inventory.AddItemResult(removed, removed.Count, Seyren.System.Inventory.ItemKind.Unique);
        }

        public Seyren.System.Inventory.AddItemResult RemoveItem(IItem item) => RemoveItem(item?.ID);

        // ── Query ─────────────────────────────────────────────────────────

        public bool ContainsItem(string itemId) => slots.Any(s => s != null && s.ID == itemId);

        public int GetItemCountByTypeID(IItem item) => GetItemCountByTypeID(item?.TypeId);
        public int GetItemCountByTypeID(string itemTypeId) =>
            slots.Where(s => s != null && s.TypeId == itemTypeId).Sum(s => s.Count);

        public bool CanAddItem(IItem item) => !IsFull;

        public int FindItemSlot(IItem item) => FindItemSlot(item?.ID);
        public int FindItemSlot(string itemId)
        {
            for (int i = 0; i < slots.Length; i++)
                if (slots[i] != null && slots[i].ID == itemId) return i;
            return -1;
        }

        public int[] FindAllItemSlots(string itemTypeId) =>
            slots.Select((s, i) => (s, i))
                 .Where(t => t.s != null && t.s.TypeId == itemTypeId)
                 .Select(t => t.i).ToArray();

        public int FindEmptySlot()
        {
            for (int i = 0; i < slots.Length; i++)
                if (slots[i] == null) return i;
            return -1;
        }

        public int[] FindAllEmptySlots() =>
            slots.Select((s, i) => (s, i)).Where(t => t.s == null).Select(t => t.i).ToArray();

        // ── Manipulation ──────────────────────────────────────────────────

        public void SwapSlots(int slotA, int slotB)
        {
            if (slotA < 0 || slotA >= slots.Length) return;
            if (slotB < 0 || slotB >= slots.Length) return;
            var tmp = slots[slotA];
            slots[slotA] = slots[slotB];
            slots[slotB] = tmp;
            OnInventoryChanged?.Invoke(this);
        }

        public int MoveItems(int fromSlot, int toSlot, int quantity = 0)
        {
            var item = GetSlot(fromSlot);
            if (item == null || GetSlot(toSlot) != null) return 0;
            SetSlot(toSlot, item);
            SetSlot(fromSlot, null);
            return 1;
        }

        public void Clear()
        {
            for (int i = 0; i < slots.Length; i++)
                SetSlot(i, null);
        }

        public void ClearSlot(int slotIndex) => SetSlot(slotIndex, null);

        // ── Enumeration helpers ───────────────────────────────────────────

        public List<IItem> GetUniqueItems() =>
            slots.Where(s => s != null).Distinct().ToList();

        public List<IItem> GetItemsByRarity(int rarity) =>
            slots.Where(s => s != null && s.Rarity == rarity).ToList();

        public void SortInventory(Seyren.System.Inventory.InventorySortType sortType)
        {
            IItem[] filled = slots.Where(s => s != null).ToArray();
            IItem[] sorted = sortType switch
            {
                Seyren.System.Inventory.InventorySortType.Name     => filled.OrderBy(i => i.Name).ToArray(),
                Seyren.System.Inventory.InventorySortType.Rarity   => filled.OrderByDescending(i => i.Rarity).ToArray(),
                Seyren.System.Inventory.InventorySortType.Quantity => filled.OrderByDescending(i => i.Count).ToArray(),
                _                                                   => filled
            };
            Array.Clear(slots, 0, slots.Length);
            for (int i = 0; i < sorted.Length; i++) slots[i] = sorted[i];
            OnInventoryChanged?.Invoke(this);
        }

        public void CompactInventory()
        {
            var filled = slots.Where(s => s != null).ToArray();
            Array.Clear(slots, 0, slots.Length);
            for (int i = 0; i < filled.Length; i++) slots[i] = filled[i];
            OnInventoryChanged?.Invoke(this);
        }

        // ── Serialization ─────────────────────────────────────────────────

        public Seyren.System.Inventory.InventoryData GetInventoryData()
        {
            var data = new Seyren.System.Inventory.InventoryData();
            data.slots = slots.Select(s => s == null ? null
                : new Seyren.System.Inventory.InventoryData.ItemStackData
                  { itemId = s.ID, quantity = s.Count }).ToArray();
            return data;
        }

        public void LoadInventoryData(Seyren.System.Inventory.InventoryData data)
        {
            // Loading requires an item factory — not available here; leave as no-op.
        }
    }
}
