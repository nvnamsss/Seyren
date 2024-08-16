using System.Collections;
using System.Collections.Generic;
using Seyren.Examples.Items;
using Seyren.System.Items;
using Seyren.System.Inventories;
using Seyren.System.Units;
using UnityEngine;


namespace Seyren.Examples.Inventories
{
    public class MultiCellInventoryManager : MonoBehaviour
    {
        public Cell cell;
        public int rows;
        public int columns;
        private Unit owner;
        private MultiCellInventory inventory;
        private Cell[][] cells;
        // on click
        // on hover
        void Awake()
        {
            inventory = new MultiCellInventory(rows, columns);
            cells = new Cell[rows][];

            for (int loop = 0; loop < rows; loop++)
            {
                cells[loop] = new Cell[columns];
                for (int loop2 = 0; loop2 < columns; loop2++)
                {
                    Cell c = Instantiate(cell, Vector3.zero, Quaternion.identity, transform);
                    c.name = $"Cell {loop} - {loop2}";
                    c.row = loop;
                    c.column = loop2;
                    c.OnClick += onCellClick;
                    c.OnHover += onCellHover;
                    c.OnOut += onCellOut;
                    cells[loop][loop2] = c;
                }
            }
        }

        public bool pickUp(MyItem item)
        {
            if (!inventory.Insert(item)) return false;
            int row = inventory.GetRowOf(item);
            int column = inventory.GetColumnOf(item);

            // update cell
            for (int loop = 0; loop < item.Width; loop++)
            {
                
            }

            for (int loop = 0; loop < item.Height; loop++)
            {
                
            }

            return true;
        }

        public bool insert(MyItem item) {
            return true;
        }

        public void removeAt(int row, int column)
        {
            inventory.Remove(row, column);
        }

        public void use(MyItem item)
        {
            switch (item.GetType())
            {
                case IConsumables c:
                    c.Consume(owner);
                    break;
                case IEquipable e:
                    e.Equip(owner);
                    break;
            }
        }

        // insert at
        
        // show popup

        // show selection

        private void onCellClick(Cell cell) {
            Debug.Log($"Click at: {cell.row} - {cell.column}");
        }

        private void onCellHover(Cell cell) {
            Debug.Log($"Hover at: {cell.row} - {cell.column}");
        }

        private void onCellOut(Cell cell) {
            Debug.Log($"Out at: {cell.row} - {cell.column}");
        }
    }


}
