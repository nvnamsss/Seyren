// using CRoaring;

using System.Collections.Generic;

namespace Algorithms
{
    public interface Group {
        public bool Add(long value);
        public bool Contains(long value);
    }    

    public class GroupSet : Group
    {
        private HashSet<long> _set = new HashSet<long>();
        public bool Add(long value) => _set.Add(value);
        public bool Contains(long value) => _set.Contains(value);
    }

    public class BloomGroup : Group
    {
        private readonly int[] _bits;
        private readonly int _size;
        private readonly int _hashFunctions;

        public BloomGroup(int size = 1024, int hashFunctions = 3)
        {
            _size = size;
            _hashFunctions = hashFunctions;
            _bits = new int[(_size + 31) / 32];
        }

        public bool Add(long value)
        {
            bool added = false;
            for (int i = 0; i < _hashFunctions; i++)
            {
                long hashLong = GetHash(value, i);
                int index = (int)(hashLong % _size);
                int arrayIndex = index / 32;
                int bitIndex = index % 32;

                if ((_bits[arrayIndex] & (1 << bitIndex)) == 0)
                {
                    added = true;
                    _bits[arrayIndex] |= (1 << bitIndex);
                }
            }
            return added;
        }

        public bool Contains(long value)
        {
            for (int i = 0; i < _hashFunctions; i++)
            {
                long hashLong = GetHash(value, i);
                int index = (int)(hashLong % _size);
                int arrayIndex = index / 32;
                int bitIndex = index % 32;

                if ((_bits[arrayIndex] & (1 << bitIndex)) == 0)
                    return false;
            }
            return true;
        }

        private long GetHash(long value, int seed)
        {
            long hash = value;
            hash = hash + seed * 0x9e3779b9L;
            hash = (hash ^ (hash >> 16)) * 0x85ebca6bL;
            hash = (hash ^ (hash >> 13)) * 0xc2b2ae35L;
            hash = hash ^ (hash >> 16);
            return hash;
        }
    }
}