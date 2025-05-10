using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Seyren.System.Common;
using Seyren.System.Units;

namespace Seyren.Algorithms.Spatial
{
    /// <summary>
    /// Representation the tree data structure in which each internal node has exactly four children
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QuadTree<T> where T : ICoordinate, IObject
    {
        /// <summary>
        /// Max item can contains
        /// </summary>
        public static int MaxItem = 6;
        /// <summary>
        /// Ancestor (parent) of the tree
        /// </summary>
        public QuadTree<T> Ancestor { get; }
        /// <summary>
        /// Descendants (childs) of the tree
        /// </summary>
        public QuadTree<T>[] Descendants { get; }
        /// <summary>
        /// The bound of the tree. Bound determine the affected range of specified tree in space. <br/>
        /// </summary>
        public Bounds Bound => _bound;
        /// <summary>
        /// List of item in the tree
        /// </summary>
        public List<T> Items { get; }
        /// <summary>
        /// Determine if this node is the leaf
        /// </summary>
        public bool IsLeaf
        {
            get
            {
                return Descendants[0] == null && Descendants[1] == null && Descendants[2] == null && Descendants[3] == null;
            }
        }
        private Bounds _bound;
        /// <summary>
        /// Initialize a new instance of 
        /// <see cref="RTree{T}"/>
        /// has the specified initial layer
        /// </summary>
        public QuadTree(Bounds bound)
        {
            Descendants = new QuadTree<T>[4];
            Items = new List<T>(MaxItem);
            _bound = bound;
        }

        /// <summary>
        /// Initialize a new instanne of 
        /// <see cref="QuadTree{T}"/>
        /// has the specified initial ancestor
        /// </summary>
        /// <param name="ancestor"></param>
        public QuadTree(Bounds bound, QuadTree<T> ancestor) : this(bound)
        {
            Ancestor = ancestor;
        }

        /// <summary>
        /// Add a new item to the tree
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(T item)
        {
            QuadTree<T> candidate = ChooseLeaf(item);
            if (candidate != null)
            {
                if (candidate.Items.Count > MaxItem)
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
            QuadTree<T> candidate = ChooseLeaf(item);
            for (int loop = candidate.Items.Count - 1; loop > 0; loop--)
            {
                if (candidate.Items[loop].ID == item.ID)
                {
                    candidate.Items.RemoveAt(loop);
                }
            }
        }

        private void RemoveItem(T item, QuadTree<T> candidate)
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
            QuadTree<T> candidate = ChooseLeaf(new Bounds(item.Location, item.Size));
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

            for (int loop = 0; loop < 4; loop++)
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

            for (int loop = 0; loop < 4; loop++)
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
        private QuadTree<T> ChooseLeaf(T item)
        {
            Bounds bounds = new Bounds(item.Location, item.Size);
            return ChooseLeaf(bounds);
        }

        private QuadTree<T> ChooseLeaf(Bounds bounds)
        {
            if (IsLeaf && Bound.Intersects(bounds)) return this;
            QuadTree<T> candidate = null;
            float minDistance = 0;
            foreach (QuadTree<T> descendant in Descendants)
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
            Vector2[] directions = new Vector2[4] { new Vector2(-size.x, size.y), new Vector2(size.x, size.y), new Vector2(size.x, -size.y), new Vector2(-size.x, -size.y) };
            for (int loop = 0; loop < 4; loop++)
            {
                Vector3 direction = directions[loop];
                Bounds bound = new Bounds(center + direction, size);
                Descendants[loop] = new QuadTree<T>(bound, Descendants[loop]);
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
        public void Iterate(Match<QuadTree<T>> apply)
        {
            if (!apply.Invoke(this)) return;

            for (int loop = 0; loop < Descendants.Length; loop++)
            {
                Descendants[loop].Iterate(apply);
            }
        }
    }
}