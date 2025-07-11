using NUnit.Framework;
using System.Collections.Generic;
using Seyren.Algorithms;

public class RandomSetTests
{
    [Test]
    public void EmptySet_ReturnsDefault()
    {
        var set = new RandomSet<int>();
        Assert.AreEqual(0, set.Get());
    }

    [Test]
    public void SingleItem_AlwaysReturnsItem()
    {
        var set = new RandomSet<int>();
        set.Add(42, 1.0f);
        
        for (int i = 0; i < 100; i++)
        {
            Assert.AreEqual(42, set.Get());
        }
    }

    [Test]
    public void Constructor_WithItemsAndChances_InitializesCorrectly()
    {
        var items = new List<int> { 1, 2, 3 };
        var chances = new List<float> { 1f, 1f, 1f };
        
        var set = new RandomSet<int>(items, chances);
        
        // Test multiple gets to ensure all items are accessible
        var results = new HashSet<int>();
        for (int i = 0; i < 1000; i++)
        {
            results.Add(set.Get());
        }
        
        Assert.AreEqual(3, results.Count);
        Assert.IsTrue(results.Contains(1));
        Assert.IsTrue(results.Contains(2));
        Assert.IsTrue(results.Contains(3));
    }

    [Test]
    public void Clear_RemovesAllItems()
    {
        var set = new RandomSet<int>();
        set.Add(1, 1.0f);
        set.Add(2, 1.0f);
        
        set.Clear();
        Assert.AreEqual(0, set.Get());
    }

    [Test]
    public void WeightedDistribution_RespectsWeights()
    {
        var set = new RandomSet<int>();
        set.Add(1, 80f);
        set.Add(2, 20f);

        int[] counts = new int[3];
        int iterations = 10000;

        for (int i = 0; i < iterations; i++)
        {
            counts[set.Get()]++;
        }

        // With 80% weight, item 1 should appear roughly 8000 times (±500 for randomness)
        Assert.Greater(counts[1], iterations * 0.75f);
        Assert.Less(counts[1], iterations * 0.85f);
        
        // With 20% weight, item 2 should appear roughly 2000 times (±500 for randomness)
        Assert.Greater(counts[2], iterations * 0.15f);
        Assert.Less(counts[2], iterations * 0.25f);
    }
}
