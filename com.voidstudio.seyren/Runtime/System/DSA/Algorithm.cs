using System;
using System.Collections.Generic;

namespace Algorithms
{
    public interface ISortable32 {
        int Weight(); 
    }

    public interface ISortable64{
        long Weight();
    }

    public class Algorithm
    {
        public static int BinarySearch(List<ISortable32> arr, int target)
        {
            return binarySearch32(arr, target, 0, arr.Count - 1);
        }   

        public static int BinarySearch(List<ISortable64> arr, long target) {
            return binarySearch64(arr, target, 0, arr.Count - 1);
        }

        private static int binarySearch32(List<ISortable32> arr, int target, int l, int r)
        {
            if (l <= r)
            {
                int mid = l + (r - l) / 2;
                if (arr[mid].Weight() == target) return mid;
                if (arr[mid].Weight() > target) return binarySearch32(arr, target, l, mid - 1);

                return binarySearch32(arr, target, mid + 1, r);
            }

            return -1;
        }

                private static int binarySearch64(List<ISortable64> arr, long target, int l, int r)
        {
            if (l <= r)
            {
                int mid = l + (r - l) / 2;
                if (arr[mid].Weight() == target) return mid;
                if (arr[mid].Weight() > target) return binarySearch64(arr, target, l, mid - 1);

                return binarySearch64(arr, target, mid + 1, r);
            }

            return -1;
        }
    }
}