using NUnit.Framework;
using UnityEngine;
using Seyren.Algorithms.Spatial;
using Seyren.System.Units;
using System;

namespace Seyren.Tests.Algorithms
{
    public class TestItem : ICoordinate, IObject
    {
        public string ID => id;
        public Vector3 Location { get; set; }
        public Vector3 Size { get; set; }

        public Quaternion Rotation => Quaternion.identity; // Simple default implementation

        public ObjectStatus ObjectStatus { 
            get => _objectStatus; 
            set => _objectStatus = value; 
        }

        public bool IsActive =>true;
        
        private string id;
        private ObjectStatus _objectStatus = ObjectStatus.None; // Default to active

        public TestItem(Vector3 location, Vector3 size)
        {
            // format: test_uuid
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            this.id = $"test_{myuuidAsString}";
            Location = location;
            Size = size;
        }

    }

    public class QuadTreeTests
    {
        private QuadTree<TestItem> quadTree;
        private readonly Vector3 defaultSize = new Vector3(1, 1, 1);

        [SetUp]
        public void Setup()
        {
            // Create a quad tree with bounds centered at (0,0,0) with size 100x100x100
            quadTree = new QuadTree<TestItem>(new Bounds(Vector3.zero, new Vector3(100, 100, 100)));
        }

        [Test]
        public void AddItem_SingleItem_ShouldBeStoredInTree()
        {
            var item = new TestItem(Vector3.zero, defaultSize);
            quadTree.AddItem(item);

            var results = quadTree.Search(Vector3.zero, _ => true);
            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0], Is.EqualTo(item));
        }

        [Test]
        public void AddItem_MultipleItems_ShouldTriggerSplit()
        {
            // Add more items than MaxItem to trigger split
            for (int i = 0; i < QuadTree<TestItem>.MaxItem + 1; i++)
            {
                var item = new TestItem(new Vector3(i, 0, 0), defaultSize);
                quadTree.AddItem(item);
            }

            // Search at the position of first and last items
            var firstResults = quadTree.Search(Vector3.zero, _ => true);
            var lastResults = quadTree.Search(new Vector3(QuadTree<TestItem>.MaxItem, 0, 0), _ => true);

            Assert.That(firstResults.Count, Is.GreaterThan(0));
            Assert.That(lastResults.Count, Is.GreaterThan(0));
        }

        [Test]
        public void RemoveItem_ExistingItem_ShouldBeRemoved()
        {
            var item = new TestItem(Vector3.zero, defaultSize);
            quadTree.AddItem(item);
            quadTree.RemoveItem(item);

            var results = quadTree.Search(Vector3.zero, _ => true);
            Assert.That(results.Count, Is.EqualTo(0));
        }

        [Test]
        public void Search_ByPoint_ShouldReturnItemsAtLocation()
        {
            var item1 = new TestItem(new Vector3(0, 0, 0), new Vector3(2, 2, 2));
            var item2 = new TestItem(new Vector3(5, 0, 0), new Vector3(2, 2, 2));
            
            quadTree.AddItem(item1);
            quadTree.AddItem(item2);

            var results = quadTree.Search(new Vector3(1, 0, 0), _ => true);
            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0], Is.EqualTo(item1));
        }

        [Test]
        public void Search_ByBounds_ShouldReturnItemsInBounds()
        {
            var item1 = new TestItem(new Vector3(0, 0, 0), defaultSize);
            var item2 = new TestItem(new Vector3(2, 0, 0), defaultSize);
            var item3 = new TestItem(new Vector3(10, 0, 0), defaultSize);
            
            quadTree.AddItem(item1);
            quadTree.AddItem(item2);
            quadTree.AddItem(item3);

            var searchBounds = new Bounds(new Vector3(1, 0, 0), new Vector3(4, 4, 4));
            var results = quadTree.Search(searchBounds, _ => true);
            
            Assert.That(results.Count, Is.EqualTo(2));
            Assert.That(results, Does.Contain(item1));
            Assert.That(results, Does.Contain(item2));
        }

        [Test]
        public void UpdateItem_MovingItem_ShouldUpdateLocation()
        {
            var item = new TestItem(Vector3.zero, defaultSize);
            quadTree.AddItem(item);

            var oldLocation = item.Location;
            item.Location = new Vector3(10, 0, 0);
            quadTree.UpdateItem(item, oldLocation);

            var resultsAtOld = quadTree.Search(Vector3.zero, _ => true);
            var resultsAtNew = quadTree.Search(new Vector3(10, 0, 0), _ => true);

            Assert.That(resultsAtOld.Count, Is.EqualTo(0));
            Assert.That(resultsAtNew.Count, Is.EqualTo(1));
            Assert.That(resultsAtNew[0], Is.EqualTo(item));
        }

        [Test]
        public void Search_WithFilter_ShouldOnlyReturnMatchingItems()
        {
            var item1 = new TestItem(Vector3.zero, new Vector3(1, 1, 1));
            var item2 = new TestItem(Vector3.zero, new Vector3(2, 2, 2));
            
            quadTree.AddItem(item1);
            quadTree.AddItem(item2);

            var results = quadTree.Search(Vector3.zero, item => item.Size.x > 1);
            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0], Is.EqualTo(item2));
        }

        [Test]
        public void AddItem_OutOfBounds_ShouldNotAdd()
        {
            var item = new TestItem(new Vector3(1000, 0, 0), defaultSize);
            quadTree.AddItem(item);

            var results = quadTree.Search(item.Location, _ => true);
            Assert.That(results.Count, Is.EqualTo(0));
        }
    }
}
