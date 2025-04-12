using System.Collections.Generic;
using UnityEngine;

namespace Seyren.Algorithms {
    public class RandomSet<T> {
        private List<T> items;
        private List<float> chances;
        private List<float> prefixSum;
        private float totalChance;

        public RandomSet() {
            items = new List<T>();
            chances = new List<float>();
            prefixSum = new List<float>();
            totalChance = 0;
        }

        public RandomSet(List<T> items, List<float> chances) {
            this.items = new List<T>(items);
            this.chances = new List<float>(chances);
            this.prefixSum = new List<float>();
            this.totalChance = 0;

            // Calculate prefix sums
            for (int i = 0; i < chances.Count; i++) {
                totalChance += chances[i];
                prefixSum.Add(totalChance);
            }
        }

        public void Add(T item, float chance) {
            items.Add(item);
            chances.Add(chance);
            totalChance += chance;
            prefixSum.Add(totalChance);
        }

        public T Get() {
            if (items.Count == 0) return default;
            if (items.Count == 1) return items[0];

            float randomValue = Random.Range(0f, totalChance);
            
            // Binary search to find the appropriate item
            int left = 0;
            int right = prefixSum.Count - 1;

            while (left < right) {
                int mid = left + (right - left) / 2;
                if (prefixSum[mid] < randomValue) {
                    left = mid + 1;
                } else {
                    right = mid;
                }
            }

            return items[left];
        }

        public void Clear() {
            items.Clear();
            chances.Clear();
            prefixSum.Clear();
            totalChance = 0;
        }
    }
}