using Unity.PerformanceTesting;
using NUnit.Framework;
using System.Collections.Generic;
using Seyren.Algorithms;

public class RandomSetPerformanceTests
{
    [Test, Performance]
    public void Constructor_Performance()
    {
        var items = new List<int>();
        var chances = new List<float>();
        
        for (int i = 0; i < 10000; i++)
        {
            items.Add(i);
            chances.Add(1.0f);
        }

        Measure.Method(() =>
        {
            var set = new RandomSet<int>(items, chances);
        })
        .WarmupCount(5)
        .MeasurementCount(100)
        .Run();
    }

    [Test, Performance]
    public void Add_Performance()
    {
        var set = new RandomSet<int>();

        Measure.Method(() =>
        {
            set.Add(1, 1.0f);
        })
        .WarmupCount(5)
        .MeasurementCount(1000)
        .SetUp(() => set.Clear())
        .Run();
    }

    [Test, Performance]
    public void Get_Performance_DifferentSizes()
    {
        int[] setSizes = { 10, 100, 1000, 10000 };

        foreach (int size in setSizes)
        {
            var set = new RandomSet<int>();
            for (int i = 0; i < size; i++)
            {
                set.Add(i, 1.0f);
            }

            Measure.Method(() =>
            {
                var item = set.Get();
            })
            .WarmupCount(5)
            .MeasurementCount(1000)
            .SampleGroup($"Get_Size_{size}")
            .Run();
        }
    }

    [Test, Performance]
    public void Get_Performance_WeightDistribution()
    {
        var uniformSet = new RandomSet<int>();
        var skewedSet = new RandomSet<int>();

        // Uniform distribution
        for (int i = 0; i < 1000; i++)
        {
            uniformSet.Add(i, 1.0f);
        }

        // Heavily skewed distribution
        for (int i = 0; i < 1000; i++)
        {
            skewedSet.Add(i, i + 1.0f);
        }

        Measure.Method(() =>
        {
            var item = uniformSet.Get();
        })
        .WarmupCount(5)
        .MeasurementCount(1000)
        .SampleGroup("Uniform_Distribution")
        .Run();

        Measure.Method(() =>
        {
            var item = skewedSet.Get();
        })
        .WarmupCount(5)
        .MeasurementCount(1000)
        .SampleGroup("Skewed_Distribution")
        .Run();
    }
}
