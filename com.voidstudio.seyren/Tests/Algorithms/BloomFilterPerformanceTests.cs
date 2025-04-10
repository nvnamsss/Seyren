using NUnit.Framework;
using Seyren.Algorithms;
using System.Linq;
using Unity.PerformanceTesting;

namespace Seyren.Tests.Editor
{
    public class BloomFilterPerformanceTests
    {
        private BloomFilter<int> _filter;
        private int[] _testData;
        private int[] _searchData;

        [SetUp]
        public void Setup()
        {
            _filter = new BloomFilter<int>(100000, 3);
            _testData = Enumerable.Range(0, 10000).ToArray();
            _searchData = Enumerable.Range(5000, 10000).ToArray();
        }

        [Test, Performance]
        public void Performance_Add()
        {
            Measure.Method(() =>
            {
                foreach (var item in _testData)
                {
                    _filter.Add(item);
                }
            })
            .WarmupCount(3)
            .MeasurementCount(5)
            .SampleGroup("BloomFilter.Add")
            .Run();
        }

        [Test, Performance]
        public void Performance_Contains()
        {
            // Pre-populate filter
            foreach (var item in _testData)
            {
                _filter.Add(item);
            }

            Measure.Method(() =>
            {
                foreach (var item in _searchData)
                {
                    _filter.Contains(item);
                }
            })
            .WarmupCount(3)
            .MeasurementCount(5)
            .SampleGroup("BloomFilter.Contains")
            .Run();
        }

        [Test, Performance]
        public void Performance_AddWithDifferentSizes()
        {
            var sizes = new[] { 1000, 10000, 100000 };
            
            foreach (var size in sizes)
            {
                _filter = new BloomFilter<int>(size, 3);
                var data = Enumerable.Range(0, size / 10).ToArray();

                Measure.Method(() =>
                {
                    foreach (var item in data)
                    {
                        _filter.Add(item);
                    }
                })
                .WarmupCount(2)
                .MeasurementCount(3)
                .SampleGroup($"BloomFilter.Add.Size{size}")
                .Run();
            }
        }
    }
}
