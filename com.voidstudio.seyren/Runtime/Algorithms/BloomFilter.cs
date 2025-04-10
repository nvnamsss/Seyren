using System;
using System.Collections;

namespace Seyren.Algorithms {
    public class BloomFilter<T> {
        private readonly BitArray _bits;
        private readonly int _hashFunctionCount;
        private readonly int _size;

        public BloomFilter(int size, int hashFunctionCount) {
            if (size <= 0) throw new ArgumentException("Size must be greater than 0");
            if (hashFunctionCount <= 0) throw new ArgumentException("Must have at least one hash function");

            _size = size;
            _hashFunctionCount = hashFunctionCount;
            _bits = new BitArray(size);
        }

        public void Add(T item) {
            if (item == null) throw new ArgumentNullException(nameof(item));

            int hash = item.GetHashCode();
            for (int i = 0; i < _hashFunctionCount; i++) {
                // Use double hashing to generate multiple hash functions
                int index = Math.Abs((hash + i * (hash >> 5)) % _size);
                _bits[index] = true;
            }
        }

        public bool Contains(T item) {
            if (item == null) throw new ArgumentNullException(nameof(item));

            int hash = item.GetHashCode();
            for (int i = 0; i < _hashFunctionCount; i++) {
                int index = Math.Abs((hash + i * (hash >> 5)) % _size);
                if (!_bits[index]) return false;
            }
            return true;
        }

        public void Clear() {
            _bits.SetAll(false);
        }
    }
}