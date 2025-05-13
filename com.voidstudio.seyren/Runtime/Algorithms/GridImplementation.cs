using System.Collections.Generic;
using UnityEngine;

namespace Seyren.Algorithms
{
    /// <summary>
    /// Example implementation of a 2D grid for A* pathfinding
    /// </summary>
    public class Grid2D : Astar.IGrid
    {
        private bool[,] walkableGrid;
        private float cellSize;
        private Vector2 gridOrigin;

        /// <summary>
        /// Creates a 2D grid
        /// </summary>
        /// <param name="width">Width of the grid in cells</param>
        /// <param name="height">Height of the grid in cells</param>
        /// <param name="cellSize">Size of each cell</param>
        /// <param name="gridOrigin">World position of the bottom-left corner</param>
        public Grid2D(int width, int height, float cellSize, Vector2 gridOrigin)
        {
            this.walkableGrid = new bool[width, height];
            this.cellSize = cellSize;
            this.gridOrigin = gridOrigin;

            // Default to all cells being walkable
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    walkableGrid[x, y] = true;
                }
            }
        }

        /// <summary>
        /// Set whether a specific cell is walkable
        /// </summary>
        public void SetWalkable(int x, int y, bool walkable)
        {
            if (IsValidCell(x, y))
                walkableGrid[x, y] = walkable;
        }

        /// <summary>
        /// Convert world position to grid cell coordinates
        /// </summary>
        public Vector2Int WorldToCell(Vector3 worldPosition)
        {
            Vector2 localPosition = new Vector2(worldPosition.x, worldPosition.z) - gridOrigin;
            int x = Mathf.FloorToInt(localPosition.x / cellSize);
            int y = Mathf.FloorToInt(localPosition.y / cellSize);
            return new Vector2Int(x, y);
        }

        /// <summary>
        /// Convert grid cell to world position (at center of cell)
        /// </summary>
        public Vector3 CellToWorld(int x, int y)
        {
            float worldX = gridOrigin.x + (x * cellSize) + (cellSize / 2);
            float worldZ = gridOrigin.y + (y * cellSize) + (cellSize / 2);
            return new Vector3(worldX, 0, worldZ);
        }

        private bool IsValidCell(int x, int y)
        {
            return x >= 0 && x < walkableGrid.GetLength(0) && 
                   y >= 0 && y < walkableGrid.GetLength(1);
        }

        #region IGrid Implementation

        public List<Vector3> GetNeighbors(Vector3 position)
        {
            List<Vector3> neighbors = new List<Vector3>();
            Vector2Int cell = WorldToCell(position);
            
            // Check all 8 surrounding cells (including diagonals)
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue; // Skip self
                    
                    int checkX = cell.x + x;
                    int checkY = cell.y + y;
                    
                    // If diagonal, only allow if adjacent cells are walkable too (prevents cutting corners)
                    if (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1)
                    {
                        bool canMoveDiagonally = IsValidCell(cell.x + x, cell.y) && 
                                                IsValidCell(cell.x, cell.y + y) &&
                                                walkableGrid[cell.x + x, cell.y] &&
                                                walkableGrid[cell.x, cell.y + y];
                        
                        if (!canMoveDiagonally)
                            continue;
                    }
                    
                    if (IsValidCell(checkX, checkY) && walkableGrid[checkX, checkY])
                    {
                        neighbors.Add(CellToWorld(checkX, checkY));
                    }
                }
            }
            
            return neighbors;
        }

        public bool IsWalkable(Vector3 position)
        {
            Vector2Int cell = WorldToCell(position);
            return IsValidCell(cell.x, cell.y) && walkableGrid[cell.x, cell.y];
        }

        public float GetMovementCost(Vector3 from, Vector3 to)
        {
            // Diagonal movement costs more than cardinal movement
            return Vector3.Distance(from, to);
        }
        
        #endregion
    }
}
