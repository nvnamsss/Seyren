using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Seyren.System.Generics;
using Seyren.System.Units;

namespace Seyren.System.Spatial
{
    public class OcTree<T> where T : ICoordinate
    {
        int maxItem;
        public OcTree<T> Ancestor { get; }
        /// <summary>
        /// Descendants (childs) of the tree
        /// </summary>
        public OcTree<T>[] Descendants { get; }
        public Bounds Bound => _bound;
        /// <summary>
        /// List of item in the tree
        /// </summary>
        private List<T> Items { get; }
        /// <summary>
        /// Determine if this node is the leaf
        /// </summary>
        public bool IsLeaf
        {
            get
            {
                bool result = true;
                for (int loop = 0; loop < Descendants.Length; loop++)
                {
                    result = result && (Descendants[loop] == null);
                }
                return result;
            }
        }

        private Bounds _bound;
        public OcTree(int maxItem)
        {
            this.maxItem = maxItem;
        }

        /// <summary>
        /// Initialize a new instance of 
        /// <see cref="Octree{T}"/>
        /// has the specified initial layer
        /// </summary>
        public OcTree(Bounds bound, int maxItem = 6)
        {
            this.maxItem = maxItem;
            Descendants = new OcTree<T>[8];
            Items = new List<T>(maxItem);
            _bound = bound;
        }

        /// <summary>
        /// Initialize a new instanne of 
        /// <see cref="OcTree{T}"/>
        /// has the specified initial ancestor
        /// </summary>
        /// <param name="ancestor"></param>
        public OcTree(Bounds bound, OcTree<T> ancestor, int maxItem) : this(bound, maxItem)
        {
            Ancestor = ancestor;
        }

        /// <summary>
        /// Add a new item to the tree
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(T item)
        {
            OcTree<T> candidate = ChooseLeaf(item);
            if (candidate != null)
            {
                if (candidate.Items.Count > maxItem)
                {
                    candidate.Split();
                    candidate = candidate.ChooseLeaf(item);
                }
                candidate.Items.Add(item);
            }

        }

        /// <summary>
        /// Remove a specified item out of the tree
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(T item)
        {
            OcTree<T> candidate = ChooseLeaf(item);
            for (int loop = candidate.Items.Count - 1; loop > 0; loop--)
            {
                if (candidate.Items[loop].Equals(item))
                {
                    candidate.Items.RemoveAt(loop);
                }
            }
        }

        private void RemoveItem(T item, OcTree<T> candidate)
        {
            for (int loop = candidate.Items.Count - 1; loop > 0; loop--)
            {
                if (candidate.Items[loop].Equals(item))
                {
                    candidate.Items.RemoveAt(loop);
                }
            }
        }

        public void UpdateItem(T item, Vector3 oldLocation)
        {
            OcTree<T> candidate = ChooseLeaf(new Bounds(item.Location, item.Size));
            if (candidate.Bound.Contains(item.Location))
            {
                return;
            }

            RemoveItem(item, candidate);
            AddItem(item);
        }

        /// <summary>
        /// Return all 
        /// <see cref="SpaceObject{T}"/>
        /// whose bound contains the search location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<T> Search(Vector3 location, Match<T> filter)
        {
            List<T> objs = new List<T>();
            if (IsLeaf)
            {
                for (int loop = 0; loop < Items.Count; loop++)
                {
                    Bounds bounds = new Bounds(Items[loop].Location, Items[loop].Size);
                    if (bounds.Contains(location) && filter(Items[loop]))
                    {
                        objs.Add(Items[loop]);
                    }
                }

                return objs;
            }

            for (int loop = 0; loop < 8; loop++)
            {
                if (Descendants[loop].Bound.Contains(location))
                {
                    objs.AddRange(Descendants[loop].Search(location, filter));
                }
            }

            return objs;
        }

        public List<T> Search(Bounds b, Match<T> filter)
        {
            List<T> objs = new List<T>();
            if (IsLeaf)
            {
                for (int loop = 0; loop < Items.Count; loop++)
                {

                    Bounds bounds = new Bounds(Items[loop].Location, Items[loop].Size);
                    if (bounds.Intersects(bounds) && filter(Items[loop]))
                    {
                        objs.Add(Items[loop]);
                    }
                }

                return objs;
            }

            for (int loop = 0; loop < 8; loop++)
            {
                if (Descendants[loop].Bound.Intersects(b))
                {
                    objs.AddRange(Descendants[loop].Search(b, filter));
                }
            }

            return objs;
        }

        /// <summary>
        /// Choose a leaf that suitable which an item
        /// </summary>
        /// <returns></returns>
        private OcTree<T> ChooseLeaf(T item)
        {
            Bounds bounds = new Bounds(item.Location, item.Size);
            return ChooseLeaf(bounds);
        }

        private OcTree<T> ChooseLeaf(Bounds bounds)
        {
            if (IsLeaf && Bound.Intersects(bounds)) return this;
            OcTree<T> candidate = null;
            float minDistance = 0;
            foreach (OcTree<T> descendant in Descendants)
            {
                if (descendant != null && descendant.Bound.Intersects(bounds))
                {
                    if (candidate == null)
                    {
                        candidate = descendant;
                        minDistance = Vector3.Distance(bounds.center, candidate.Bound.center);
                    }
                    else
                    {
                        float distance = Vector3.Distance(bounds.center, descendant.Bound.center);
                        if (distance < minDistance)
                        {
                            candidate = descendant;
                            minDistance = distance;
                        }
                    }
                }
            }

            if (candidate != null) return candidate.ChooseLeaf(bounds);
            return null;
        }

        private void Split()
        {
            Vector3 size = Bound.size / 2;
            Vector3 center = Bound.center;
            Vector3[] directions = new Vector3[8] {
                new Vector3(-size.x, size.y, size.z),
                new Vector3(size.x, size.y, size.z),
                new Vector3(size.x, -size.y, size.z),
                new Vector3(-size.x, -size.y, size.z),
                new Vector3(-size.x, size.y, -size.z),
                new Vector3(size.x, size.y, -size.z),
                new Vector3(size.x, -size.y, -size.z),
                new Vector3(-size.x, -size.y, -size.z),
            };

            for (int loop = 0; loop < 8; loop++)
            {
                Vector3 direction = directions[loop];
                Bounds bound = new Bounds(center + direction, size);
                Descendants[loop] = new OcTree<T>(bound, Descendants[loop], maxItem);
            }

            for (int loop = Items.Count - 1; loop > 0; loop--)
            {
                AddItem(Items[loop]);
                Items.RemoveAt(loop);
            }
        }

        /// <summary>
        /// Iterate over the tree and apply the change to nodes that fit with the condition.
        /// </summary>
        /// <param name="apply"></param>
        public void Iterate(Match<OcTree<T>> apply)
        {
            if (!apply.Invoke(this)) return;

            for (int loop = 0; loop < Descendants.Length; loop++)
            {
                Descendants[loop].Iterate(apply);
            }
        }

    }
}