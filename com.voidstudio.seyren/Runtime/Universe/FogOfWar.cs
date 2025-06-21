using System;
using System.Collections.Generic;
using Seyren.Algorithms;
using Seyren.System.Units;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityEngine;

namespace Seyren.Universe
{

    /*
    Need passive doodad and active doodad
    new fomular to calculate fog effectively, concrete, the reveal rate must summable
    beside the doodad, new interface revealer will be added.
    revealer do the samething as doodad but in the opposite side
    doodads will be placed in each fog instead of fog of war
    */
    public interface Doodad : ICoordinate
    {
        void Block(FogOfWar fow, Sight sight);
    }

    public interface Sight : ICoordinate
    {
        void Reveal(FogOfWar fow);
        bool Revealable(Vector3 location);
    }

    public class Tree : Doodad
    {
        FogOfWar fogOfWar;
        public Vector3 Location => location;

        public Vector3 Size { get; set; }

        public Quaternion Rotation => throw new NotImplementedException();
        public Vector3 Forward => throw new NotImplementedException();
        Vector3 location;
        float maxDistanceFOV;
        float lerpDensity;
        public Tree(Vector3 location, float maxDistanceFOV, float lerpDensity)
        {
            this.location = location;
            this.maxDistanceFOV = maxDistanceFOV;
            this.lerpDensity = lerpDensity;
        }

        public void Block(FogOfWar fow, Sight sight)
        {
            Vector3 blockDirection = location - sight.Location;
            Vector3 loc = location;
            blockDirection.Normalize();
            float wide = 0;
            float deltaWide = Mathf.Max(1f - Vector3.Distance(location, sight.Location) / maxDistanceFOV, 0); // wider when closer
            // Debug.Log($"angle: {angle * Mathf.Rad2Deg} - block: {blockDirection} - dwidth: {deltaWidth} - dheight: {deltaHeight} - dwide: {deltaWide}");
            Vector3 norm = Vector3.Cross(Vector3.up, loc).normalized;
            
            // Looping till sight cannot reveal the loc
            while (sight.Revealable(loc))
            {
                loc += blockDirection;
                Vector3 wideHigh = loc + norm * wide;
                Vector3 wideLow = loc - norm * wide;

                float t = 0;
                while (t <= 1)
                {
                    Vector3 lerp = Vector3.Lerp(wideLow, wideHigh, t);
                    Fog fog = fow.GetFog(lerp);
                    if (fog != null)
                    {
                        fog.blocked = fow.version;
                        fog.MarkBlocked();
                    }
                    t += lerpDensity;
                }

                wide += deltaWide;
            }
        }

    }

    public class Bush : Doodad
    {
        public Vector3 Location => location;

        public Vector3 Size { get; set; }

        public Quaternion Rotation => throw new NotImplementedException();
        public Vector3 Forward => throw new NotImplementedException();
        Vector3 location;
        public Bush(Vector3 location, Vector3 size)
        {
            this.location = location;
            this.Size = size;
        }

        public void Block(FogOfWar fow, Sight sight)
        {
            Bounds bounds = new Bounds(location, Size);
            if (bounds.Contains(sight.Location))
            {
                Debug.Log("reveal loc in bush");
                return;
            }

            int r = (int)(Size.x / fow.gridSize.x);
            List<int[]> grids = CircleDrawing.HaflIntegerCentered(r);

            for (int loop = 0; loop < grids.Count; loop++)
            {
                int width = grids[loop][0];
                int depth = grids[loop][1];

                for (int loop2 = -width; loop2 <= width; loop2++)
                {
                    Fog fogUpper = fow.GetFog(new Vector3(Location.x + loop2 * fow.gridSize.x, 0, Location.z + depth * fow.gridSize.y));
                    Fog fogLower = fow.GetFog(new Vector3(Location.x + loop2 * fow.gridSize.x, 0, Location.z - depth * fow.gridSize.y));
                    if (fogUpper != null)
                    {
                        fogUpper.blocked = fow.version;
                        fogUpper.MarkBlocked();
                    }
                    if (fogLower != null)
                    {
                        fogLower.blocked = fow.version;
                        fogLower.MarkBlocked();
                    }
                }
            }

        }
    }

    public class AreaSight : Sight
    {
        public int radius;
        public Vector3 Location { get; }

        public Vector3 Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Quaternion Rotation => throw new NotImplementedException();
        public Vector3 Forward => throw new NotImplementedException();

        public AreaSight(Vector3 location, int radius)
        {
            Location = location;
            this.radius = radius;
        }

        public void Reveal(FogOfWar fow)
        {
            // int xc = (int)((location.x - pivot.x) / gridSize.x);
            // int yc = (int)((location.z - pivot.z) / gridSize.z);
            int r = (int)(radius / fow.gridSize.x);
            List<int[]> grids = CircleDrawing.HaflIntegerCentered(r);
            List<Doodad> doodads = new List<Doodad>();

            for (int loop = 0; loop < grids.Count; loop++)
            {
                int width = grids[loop][0];
                int depth = grids[loop][1];

                for (int loop2 = -width; loop2 <= width; loop2++)
                {
                    Fog fogUpper = fow.GetFog(new Vector3(Location.x + loop2 * fow.gridSize.x, 0, Location.z + depth * fow.gridSize.y));
                    Fog fogLower = fow.GetFog(new Vector3(Location.x + loop2 * fow.gridSize.x, 0, Location.z - depth * fow.gridSize.y));
                    if (fogUpper != null)
                    {
                        fogUpper.IncrReveal();
                        doodads.AddRange(fogUpper.Doodads());
                    }
                    if (fogLower != null)
                    {
                        fogLower.IncrReveal();
                        doodads.AddRange(fogLower.Doodads());

                    }
                }
            }

            foreach (var doodad in doodads)
            {
                doodad.Block(fow, this);
            }
        }

        public bool Revealable(Vector3 location)
        {
            return Vector3.Distance(location, this.Location) <= radius;
        }
    }


    public class Fog : ICoordinate
    {
        public int blocked;
        public Vector3 Location => location;

        public Vector3 Size { get => throw new global::System.NotImplementedException(); set => throw new global::System.NotImplementedException(); }

        public Quaternion Rotation => throw new global::System.NotImplementedException();
        public Vector3 Forward => throw new global::System.NotImplementedException();
        int reveal;
        List<Doodad> doodads;
        Vector3 location;
        Vector3 size;
        public Fog(Vector3 location, Vector3 size)
        {
            this.location = location;
            this.size = size;
            reveal = 0;
            // lastUpdate = DateTimeOffset.Now.ToUnixTimeSeconds();
            doodads = new List<Doodad>();
        }

    
        public void MarkBlocked()
        {
            reveal = 128;
        }

        public int IncrReveal()
        {
            reveal = Math.Min(reveal + 20, 255);
            // lastUpdate = DateTimeOffset.Now.ToUnixTimeSeconds();
            return reveal;
        }

        public int GetReveal()
        {
            // long now = DateTimeOffset.Now.ToUnixTimeSeconds();
            // long now = 1;   
            reveal = Math.Max(0, reveal - 10);
            // lastUpdate = now;
            return reveal;
        }

        public List<Doodad> Doodads()
        {
            return doodads;
        }

        public void AddDoodad(Doodad doodad)
        {
            doodads.Add(doodad);
        }
    }

    public class FogOfWar
    {
        public Fog[,] fogs;
        public int width;
        public int height;
        public Vector3 gridSize;
        public Vector3 pivot;
        public int version;
        public FogOfWar(int w, int h, Vector3 gridSize)
        {
            this.width = w;
            this.height = h;
            this.gridSize = gridSize;

            int wd2 = w / 2;
            int hd2 = h / 2;

            pivot = new Vector3(-wd2 * gridSize.x, 0, -hd2 * gridSize.z);
            fogs = new Fog[w, h];
            Bounds bounds = new Bounds(Vector3.zero, new Vector3(w, h, 0));
        }

        public void AddDoodad(Doodad doodad)
        {
            Fog fog = GetFog(doodad.Location);
            if (fog != null)
            {
                fog.AddDoodad(doodad);
            }
        }

        public int Get(int w, int h)
        {
            return fogs[w, h].GetReveal();
        }

        public int GetAndReset(int w, int h)
        {
            fogs[w, h].blocked = 0;
            return fogs[w, h].GetReveal();
        }

        public void Reset()
        {
            version = 0;
        }

        public void Reveal(Vector3 location, float size)
        {
            version++;
            // Bounds bounds = new Bounds(location, new Vector3(size, size, size));
            // List<Doodad> doodads = doodadTree.Search(bounds, (f) =>
            // {
            //     return true;
            // });

            // for (int loop = 0; loop < doodads.Count; loop++)
            // {
            //     doodads[loop].Block(location, new Vector3(size, size, size));
            // }

            // pick adjacency grid  
            // int xc = (int)((location.x - pivot.x) / gridSize.x);
            // int yc = (int)((location.z - pivot.z) / gridSize.z);
            // int r = (int)(size / gridSize.x);
            // List<int[]> grids = CircleDrawing.HaflIntegerCentered(r);
            // for (int loop = 0; loop < grids.Count; loop++)
            // {
            //     int x = grids[loop][0];
            //     int y = grids[loop][1];

            //     for (int loop2 = -x; loop2 <= x; loop2++)
            //     {
            //         int dx = xc + loop2;
            //         int dyp = yc + y;
            //         int dyn = yc - y;
            //         if (y == 0)
            //         {
            //             reveal(dx, dyp);
            //         }
            //         else
            //         {
            //             reveal(dx, dyp);
            //             reveal(dx, dyn);
            //         }
            //     }
            // }

            // // reveal adjacency grid

        }

        private bool reveal(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return false;
            if (fogs[x, y].blocked == version)
            {
                fogs[x, y].MarkBlocked();
                // fogs[x, y].blocked = false;
                return false;
            }
            fogs[x, y].IncrReveal();
            // fogs[x, y].blocked = false;
            return true;
        }

        public Fog GetFog(Vector3 location)
        {
            int w = (int)((location.x - pivot.x) / gridSize.x);
            int h = (int)((location.z - pivot.z) / gridSize.z);
            if (w < 0 || w >= fogs.GetLength(0) || h < 0 || h >= fogs.GetLength(1))
            {
                return null;
            }

            return fogs[w, h];
        }

    }
}