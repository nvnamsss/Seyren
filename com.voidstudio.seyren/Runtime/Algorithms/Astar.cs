using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.Algorithms
{
    /// <summary>
    /// Implementation of the A* pathfinding algorithm.
    /// </summary>
    public class Astar
    {
        /// <summary>
        /// Node class for A* algorithm
        /// </summary>
        public class Node : IComparable<Node>
        {
            public Vector3 Position { get; private set; }
            public Node Parent { get; set; }
            public float GCost { get; set; } // Cost from start to current node
            public float HCost { get; set; } // Heuristic cost (estimated cost from current to goal)
            public float FCost => GCost + HCost; // Total cost

            public Node(Vector3 position)
            {
                Position = position;
                Parent = null;
                GCost = 0;
                HCost = 0;
            }

            public int CompareTo(Node other)
            {
                // Compare nodes based on FCost for priority queue
                int fCostComparison = FCost.CompareTo(other.FCost);
                if (fCostComparison != 0)
                    return fCostComparison;

                // If FCost is equal, prefer the node with lower HCost (closer to goal)
                return HCost.CompareTo(other.HCost);
            }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                    return false;

                Node other = (Node)obj;
                return Position == other.Position;
            }

            public override int GetHashCode()
            {
                return Position.GetHashCode();
            }
        }

        /// <summary>
        /// Interface for grid-based pathfinding
        /// </summary>
        public interface IGrid
        {
            List<Vector3> GetNeighbors(Vector3 position);
            bool IsWalkable(Vector3 position);
            float GetMovementCost(Vector3 from, Vector3 to);
        }

        /// <summary>
        /// Find path from start to goal on the specified grid
        /// </summary>
        /// <param name="grid">The grid to navigate</param>
        /// <param name="start">Start position</param>
        /// <param name="goal">Goal position</param>
        /// <returns>List of positions forming the path, or null if no path found</returns>
        public static List<Vector3> FindPath(IGrid grid, Vector3 start, Vector3 goal)
        {
            if (grid == null)
                throw new ArgumentNullException(nameof(grid));

            // Check if start or goal are not walkable
            if (!grid.IsWalkable(start) || !grid.IsWalkable(goal))
                return null;

            // Initialize open and closed sets
            var openSet = new SortedSet<Node>(new NodeComparer());
            var closedSet = new HashSet<Vector3>();
            var nodeMap = new Dictionary<Vector3, Node>();

            // Create start node and add to open set
            Node startNode = new Node(start);
            startNode.HCost = CalculateHeuristic(start, goal);
            openSet.Add(startNode);
            nodeMap[start] = startNode;

            while (openSet.Count > 0)
            {
                // Get the node with lowest FCost
                Node current = openSet.Min;
                openSet.Remove(current);

                // Check if goal reached
                if (Vector3.Distance(current.Position, goal) < 0.1f) // Threshold for reaching goal
                {
                    return ReconstructPath(current);
                }

                // Add current to closed set
                closedSet.Add(current.Position);

                // Process neighbors
                foreach (var neighborPos in grid.GetNeighbors(current.Position))
                {
                    // Skip if not walkable or in closed set
                    if (!grid.IsWalkable(neighborPos) || closedSet.Contains(neighborPos))
                        continue;

                    // Calculate costs
                    float gCost = current.GCost + grid.GetMovementCost(current.Position, neighborPos);

                    Node neighborNode;
                    bool isNewNode = false;

                    // Create or get neighbor node
                    if (!nodeMap.TryGetValue(neighborPos, out neighborNode))
                    {
                        neighborNode = new Node(neighborPos);
                        neighborNode.HCost = CalculateHeuristic(neighborPos, goal);
                        isNewNode = true;
                        nodeMap[neighborPos] = neighborNode;
                    }

                    // If new path to neighbor is better, update it
                    if (isNewNode || gCost < neighborNode.GCost)
                    {
                        neighborNode.Parent = current;
                        neighborNode.GCost = gCost;

                        // Add to open set if not already there
                        if (isNewNode)
                        {
                            openSet.Add(neighborNode);
                        }
                        else
                        {
                            // Force resort in the priority queue
                            openSet.Remove(neighborNode);
                            openSet.Add(neighborNode);
                        }
                    }
                }
            }

            // No path found
            return null;
        }

        /// <summary>
        /// Calculate heuristic cost from position to goal
        /// </summary>
        private static float CalculateHeuristic(Vector3 position, Vector3 goal)
        {
            // Using straight-line distance as heuristic (Euclidean distance)
            return Vector3.Distance(position, goal);
        }

        /// <summary>
        /// Reconstruct path from goal node to start node
        /// </summary>
        private static List<Vector3> ReconstructPath(Node goalNode)
        {
            List<Vector3> path = new List<Vector3>();
            Node current = goalNode;

            // Add all positions from goal to start
            while (current != null)
            {
                path.Add(current.Position);
                current = current.Parent;
            }

            // Reverse to get path from start to goal
            path.Reverse();
            return path;
        }

        /// <summary>
        /// Comparer for the sorted set to properly compare nodes
        /// </summary>
        private class NodeComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                if (x.FCost != y.FCost)
                {
                    return x.FCost.CompareTo(y.FCost);
                }
                
                if (x.HCost != y.HCost)
                {
                    return x.HCost.CompareTo(y.HCost);
                }

                // If both FCost and HCost are equal, compare positions to ensure consistent ordering
                if (x.Position.x != y.Position.x)
                    return x.Position.x.CompareTo(y.Position.x);
                if (x.Position.y != y.Position.y)
                    return y.Position.y.CompareTo(y.Position.y);
                return x.Position.z.CompareTo(y.Position.z);
            }
        }

        /// <summary>
        /// Smooth the generated path to make it more natural
        /// </summary>
        /// <param name="path">Raw path generated by A*</param>
        /// <param name="grid">Grid for checking line of sight</param>
        /// <returns>Smoothed path with fewer waypoints</returns>
        public static List<Vector3> SmoothPath(List<Vector3> path, IGrid grid)
        {
            if (path == null || path.Count <= 2)
                return path;

            List<Vector3> smoothedPath = new List<Vector3>();
            smoothedPath.Add(path[0]);

            int currentPointIndex = 0;
            
            while (currentPointIndex < path.Count - 1)
            {
                int furthestVisible = currentPointIndex + 1;
                
                // Look ahead as far as possible while maintaining line of sight
                for (int i = currentPointIndex + 2; i < path.Count; i++)
                {
                    if (HasLineOfSight(path[currentPointIndex], path[i], grid))
                    {
                        furthestVisible = i;
                    }
                    else
                    {
                        break;
                    }
                }
                
                smoothedPath.Add(path[furthestVisible]);
                currentPointIndex = furthestVisible;
            }
            
            return smoothedPath;
        }

        /// <summary>
        /// Check if there's a direct line of sight between two points
        /// </summary>
        private static bool HasLineOfSight(Vector3 start, Vector3 end, IGrid grid)
        {
            float distance = Vector3.Distance(start, end);
            Vector3 dir = (end - start).normalized;
            
            // Sample several points along the line
            int steps = Mathf.Max(10, Mathf.FloorToInt(distance * 2));
            float step = distance / steps;
            
            for (int i = 1; i < steps; i++)
            {
                Vector3 point = start + dir * (step * i);
                if (!grid.IsWalkable(point))
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
