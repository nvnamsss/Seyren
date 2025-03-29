// using System.Collections.Generic;
// using Seyren.System.Items;

// namespace Seyren.System.Inventories
// {

//     public class MultiCellInventory
//     {
//         struct Cell
//         {
//             public int Row;
//             public int Column;
//             public Cell(int row, int column)
//             {
//                 this.Row = row;
//                 this.Column = column;
//             }
//         }

//         public int rows;
//         public int columns;
//         private ICellItem[][] cells;
//         private Dictionary<ICellItem, Cell> items;
//         public MultiCellInventory(int rows, int columns)
//         {
//             this.rows = rows;
//             this.columns = columns;
//             items = new Dictionary<ICellItem, Cell>();
//             cells = new ICellItem[rows][];
//             for (int loop = 0; loop < rows; loop++)
//             {
//                 cells[loop] = new ICellItem[columns];
//             }
//         }

//         public bool Insert(ICellItem item)
//         {
//             // how to find the first free item in O(1) or O(log(n)) instead of (O(n))
//             for (int loop = 0; loop < rows; loop++)
//             {
//                 for (int loop2 = 0; loop2 < columns; loop2++)
//                 {
//                     if (CanInsert(item, loop, loop2))
//                     {
//                         return Insert(item, loop, loop2);
//                     }
//                 }
//             }

//             return false;
//         }

//         public bool Insert(ICellItem item, int row, int column)
//         {
//             if (!CanInsert(item, row, column)) return false;
//             for (int loop = 0; loop < item.Width; loop++)
//             {
//                 for (int loop2 = 0; loop2  < item.Height; loop2 ++)
//                 {
//                     cells[row + loop][column + loop2] = item;
//                 }
//             }

//             items.Add(item, new Cell(row, column));
//             return true;
//         }

//         public bool CanInsert(ICellItem item, int row, int column)
//         {
//             if (row < 0 || row >= rows || column < 0 || column >= columns) return false;

//             for (int loop = 0; loop < item.Width; loop++)
//             {
//                 if (cells[row + loop][column] != null) return false;
//             }

//             for (int loop = 0; loop < item.Height; loop++)
//             {
//                 if (cells[row][column + loop] != null) return false;
//             }

//             return true;
//         }

//         public bool Remove(int row, int column)
//         {
//             ICellItem item = GetItem(row, column);
//             if (item == null) return false;
//             Cell c = items[item];
//             for (int loop = 0; loop < item.Width; loop++)
//             {
//                 cells[c.Row + loop] = null;
//             }

//             for (int loop = 0; loop < item.Height; loop++)
//             {
//                 cells[c.Column + loop] = null;
//             }

//             items.Remove(item);
//             return true;
//         }

//         public ICellItem GetItem(int row, int column)
//         {
//             if (row >= rows || column >= columns) return null;

//             return cells[row][column];
//         }

//         public int GetRowOf(ICellItem item) {
//             if (item == null || !items.ContainsKey(item)) {
//                 return -1;
//             }

//             return items[item].Row;
//         }

//         public int GetColumnOf(ICellItem item) {
//             if (item == null || !items.ContainsKey(item)) {
//                 return -1;
//             }

//             return items[item].Column;
//         }
//     }
// }