using System.Collections.Generic;
using UnityEngine;

namespace Seyren.Universe
{
    internal class Quadrant
    {
        public enum Direction
        {
            East,
            North,
            West,
            South
        }

        Direction direction;
        Vector2Int origin;

        public Quadrant(Direction direction, Vector2Int origin)
        {
            this.direction = direction;
            this.origin = origin;
        }

        public Vector2Int Transform(Vector2Int tile)
        {
            Vector2Int transformed = Vector2Int.zero;
            switch (direction)
            {
                case Direction.East:
                    transformed = new Vector2Int(origin.x + tile.x, origin.y + tile.y);
                    break;
                case Direction.North:
                    transformed = new Vector2Int(origin.x - tile.y, origin.y + tile.x);
                    break;
                case Direction.West:
                    transformed = new Vector2Int(origin.x - tile.x, origin.y - tile.y);
                    break;
                case Direction.South:
                    transformed = new Vector2Int(origin.x + tile.y, origin.y - tile.x);
                    break;
            }

            return transformed;
        }
    }


    class Row
    {
        public int Depth { get; set; }
        public float StartSlope { get; set; }
        public float EndSlope { get; set; }

        int maxDepth;

        public Row(int depth, int maxDepth, float startSlope, float endSlope)
        {
            Depth = depth;
            StartSlope = startSlope;
            EndSlope = endSlope;
            this.maxDepth = maxDepth;
        }

        public List<Vector2Int> Tiles()
        {
            List<Vector2Int> tiles = new List<Vector2Int>();

            int min = RoundTiesUp(Depth * StartSlope);
            int max = RoundTiesDown(Depth * EndSlope);

            for (int i = min; i < max + 1; i++)
            {
                tiles.Add(new Vector2Int(Depth, i));
            }

            if (EndSlope == 1)
            {
                tiles.RemoveAt(tiles.Count - 1);
            }

            return tiles;
        }


        public Row Next()
        {
            return new Row(Depth + 1, maxDepth, StartSlope, EndSlope);
        }


        public bool IsProceedable()
        {
            return Depth < maxDepth;
        }

        int RoundTiesUp(float n)
        {
            return Mathf.FloorToInt(n + 0.5f);
        }

        int RoundTiesDown(float n)
        {
            return Mathf.CeilToInt(n - 0.5f);
        }

    }

    public class LightSource {
        public Vector2Int Location { get; set; }
        public int Radius { get; set; }
        public int Height { get; set; }
        public string ID => id;
        string id;

        public LightSource(string id, Vector2Int location, int radius, int height)
        {
            this.id = id;
            Location = location;
            Radius = radius;
            Height = height;
        }
    }

    public class Obstacle {
        public Vector2Int Position { get; set; }
        public int Size { get; set; }
        public string ID => id;
        string id;

        public Obstacle(string id, Vector2Int position, int size)
        {
            this.id = id;
            Position = position;
            Size = size;
        }
    }

    public class Tile {
        public TileType Type { get; set; }
        public float Height { get; set; }
        public Tile(TileType type, float height)
        {
            Type = type;
            Height = height;
        }
    }

    public enum TileType
    {
        Hidden = 0,
        Revealed = 1,
        Obstacle = 2,
    }

    public class Shadowcaster
    {
        // Map map;
        // Light sources
        int width;
        int height;
        Dictionary<string, LightSource> lightSources;
        Dictionary<string, Obstacle> obstacles;
        // Dictionary<string, Vector2Int> lightSources;
        // Obstacles
        Tile[,] grid;
        Color[] colors;

        public Shadowcaster(int width, int height)
        {
            // this.map = map;
            this.width = width;
            this.height = height;
            grid = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[y, x] = new Tile(TileType.Hidden, 0);
                }
            }

            lightSources = new Dictionary<string, LightSource>();
        }
        
        public void RegisterLightSource(LightSource lightSource)
        {
            lightSources.Add(lightSource.ID, lightSource);
        }

        // public void UpdateLightSource(string id, int x, int y)
        // {
        //     lightSources[id] = new Vector2Int(x, y);
        // }

        public void UnregisterLightSource(string id)
        {
            lightSources.Remove(id);
        }

        public void AddObstacle(int x, int y, int size)
        {
            // grid[x, y] = 2;
            for (int i = x - size; i <= x + size; i++)
            {
                for (int j = y - size; j <= y + size; j++)
                {
                    if (i < 0 || i >= width || j < 0 || j >= height)
                    {
                        continue;
                    }
                    grid[i, j].Type = TileType.Obstacle;
                }
            }
        }

        public Tile GetCell(int x, int y)
        {
            return grid[x, y];
        }

        public void Reset() {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (grid[x, y].Type != TileType.Obstacle)
                    {
                        grid[x, y].Type = TileType.Hidden;
                    }
                }
            }
        }
        
            public Color[] GetColors(float fogPlaneAlpha)
            {
                if (colors == null)
                {
                    colors = new Color[width * height];
                }

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int visibility = (int)grid[y, x].Type;
                        if (visibility == 2) {
                            visibility = 0;
                        }

                        // The reason that the darker side is the revealed ones is to let users customize fog's color
                        colors[x * height + y] =
                        new Color(
                            1 - visibility,
                            1 - visibility,
                            1 - visibility,
                            (1 - visibility) * fogPlaneAlpha);
                    }
                }

                return colors;
            }

        public void Compute()
        {
            foreach (var lightSource in lightSources)
            {
                ComputeFOV(lightSource.Value.Location, lightSource.Value.Radius, lightSource.Value.Height);
            }
        }

        private void ComputeFOV(Vector2Int origin, int lightRadius, int height)
        {
            Reveal(origin);
            // return;
            for (int i = 0; i < 4; i++)
            {
                Quadrant.Direction direction = (Quadrant.Direction)i;
                Quadrant quadrant = new Quadrant(direction, origin);

                Stack<Row> rows = new Stack<Row>();
                rows.Push(new Row(1, lightRadius, -1f, 1f));

                while (rows.Count > 0)
                {
                    Row row = rows.Pop();
                    Vector2Int? prevTile = null;
                    List<Vector2Int> tiles = row.Tiles();
                    foreach (Vector2Int tile in tiles)
                    {
                        // Debug.Log(tile.x + " " + tile.y + " " + Slope(tile));
                        if (IsWall(quadrant, tile, height) || IsSymmetric(row, tile))
                        {
                            // Debug.Log("Reveal");
                            Reveal(quadrant, tile, lightRadius);
                        }

                        if (IsWall(quadrant, prevTile, height) && IsFloor(quadrant, tile))
                        {
                            row.StartSlope = Slope(tile);
                        }

                        if (IsFloor(quadrant, prevTile) && IsWall(quadrant, tile, height))
                        {
                            if (row.IsProceedable())
                            {
                                Row nextRow = row.Next();
                                nextRow.EndSlope = Slope(tile);
                                // Debug.Log(nextRow.Depth + " " + nextRow.StartSlope + " " + nextRow.EndSlope);
                                rows.Push(nextRow);
                            }


                        }

                        prevTile = tile;
                    }

                    if (IsFloor(quadrant, prevTile))
                    {
                        // Debug.Log("Push");
                        if (row.IsProceedable())
                        {
                            rows.Push(row.Next());
                        }
                    }
                }
            }
        }

        private float Slope(Vector2Int tile)
        {
            return ((2.0f * tile.y) - 1.0f) / (2.0f * tile.x);
        }

        private bool IsWall(Quadrant quadrant, Vector2Int? tile, int height)
        {
            if (!tile.HasValue)
            {
                return false;
            }

            Vector2Int location = quadrant.Transform(tile.Value);
            if (location.x < 0 || location.x >= width || location.y < 0 || location.y >= height)
            {
                return false;
            }

            return height < grid[location.x, location.y].Height && grid[location.x, location.y].Type == TileType.Obstacle;
        }

        private bool IsFloor(Quadrant quadrant, Vector2Int? tile)
        {
            if (!tile.HasValue)
            {
                return false;
            }

            Vector2Int location = quadrant.Transform(tile.Value);
            if (location.x < 0 || location.x >= width || location.y < 0 || location.y >= height)
            {
                return false;
            }

            return height > grid[location.x, location.y].Height || grid[location.x, location.y].Type != TileType.Obstacle;
        }

        private void Reveal(Vector2Int location) {
            if (location.x < 0 || location.x >= width || location.y < 0 || location.y >= height)
            {
                return;
            }

            if (grid[location.x, location.y].Type == TileType.Obstacle)
            {
                return;
            }

            grid[location.x, location.y].Type = TileType.Revealed;
        }
        private void Reveal(Quadrant quadrant, Vector2Int? tile, int sightRange)
        {
            if (tile.Value.magnitude > sightRange)
            {
                return;
            }

            Vector2Int location = quadrant != null ? quadrant.Transform(tile.Value) : tile.Value;
            if (location.x < 0 || location.x >= width || location.y < 0 || location.y >= height)
            {
                return;
            }

            if (grid[location.x, location.y].Type == TileType.Obstacle)
            {
                return;
            }

            grid[location.x, location.y].Type = TileType.Revealed;
        }

        private bool IsSymmetric(Row row, Vector2Int tile)
        {
            return tile.y >= row.Depth * row.StartSlope
                && tile.y <= row.Depth * row.EndSlope;
        }
    }
}