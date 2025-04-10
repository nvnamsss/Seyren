using System;
using NUnit.Framework;
using Seyren.Algorithms;

namespace Seyren.Tests.Editor.Algorithms
{
    public class BloomFilterTests
    {
        [Test]
        public void Constructor_WithValidParameters_CreatesInstance()
        {
            Assert.DoesNotThrow(() => new BloomFilter<string>(1000, 3));
        }

        [Test]
        public void Constructor_WithInvalidSize_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new BloomFilter<string>(0, 3));
            Assert.Throws<ArgumentException>(() => new BloomFilter<string>(-1, 3));
        }

        [Test]
        public void Constructor_WithInvalidHashFunctionCount_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new BloomFilter<string>(1000, 0));
            Assert.Throws<ArgumentException>(() => new BloomFilter<string>(1000, -1));
        }

        [Test]
        public void Add_AndContains_WorksCorrectly()
        {
            var filter = new BloomFilter<string>(1000, 3);
            filter.Add("test");
            
            Assert.IsTrue(filter.Contains("test"));
            Assert.IsFalse(filter.Contains("not_added"));
        }

        [Test]
        public void Add_NullItem_ThrowsArgumentNullException()
        {
            var filter = new BloomFilter<string>(1000, 3);
            Assert.Throws<ArgumentNullException>(() => filter.Add(null));
        }

        [Test]
        public void Contains_NullItem_ThrowsArgumentNullException()
        {
            var filter = new BloomFilter<string>(1000, 3);
            Assert.Throws<ArgumentNullException>(() => filter.Contains(null));
        }

        [Test]
        public void Clear_RemovesAllItems()
        {
            var filter = new BloomFilter<string>(1000, 3);
            filter.Add("test1");
            filter.Add("test2");
            
            filter.Clear();
            
            Assert.IsFalse(filter.Contains("test1"));
            Assert.IsFalse(filter.Contains("test2"));
        }

        [Test]
        public void MultipleItems_WorksCorrectly()
        {
            var filter = new BloomFilter<int>(1000, 3);
            for (int i = 0; i < 100; i++)
            {
                filter.Add(i);
            }

            for (int i = 0; i < 100; i++)
            {
                Assert.IsTrue(filter.Contains(i));
            }
        }
    }
}
