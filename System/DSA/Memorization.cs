using System;
using System.Collections.Generic;
using DS.Expression;

namespace DSA
{
    public interface IStorage<T> where T: IComparable
    {
        T Load();
        void Save(T value);
    }

    class WeightedWrapper<T> : ISortable64
    {
        public long W;
        public T Item;
        public WeightedWrapper(long weight, T item)
        {
            W = weight;
            Item = item;
        }

        public long Weight()
        {
            return W;
        }
    }

    class Item {
        public long W;
        public float C;
        public Item(long w, float c) {
            this.W = w;
            this.C = c;
        }
    }

    public class Memorization
    {
        private float decayTime;
        private float threshold = 25;
        private float cache;
        private List<Item> items;
        private List<WeightedWrapper<LinearPolynomial>> storages;
        public Memorization(int decayTime)
        {
            items = new List<Item>();
            storages = new List<WeightedWrapper<LinearPolynomial>>();
            this.decayTime = decayTime;
        }

        public float Load(int timestamp)
        {
            int index = Search(storages, timestamp);
            LinearPolynomial s = storages[index].Item;
            int last = items.Count;
            float value = 0;
            while (items[last].W - timestamp > decayTime) {
                value -= items[last].C;
            }
            return value;

            return s.Evaluate(timestamp);
        }

        public void Save(float value)
        {
            long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            while (storages[0].W < decayTime ) { 
                storages.RemoveAt(0);
            }

            WeightedWrapper<LinearPolynomial> last = storages[storages.Count - 1];
            LinearPolynomial s;
            if (now - last.W > threshold) {
                s = last.Item;
            } else {
                WeightedWrapper<LinearPolynomial> w = new WeightedWrapper<LinearPolynomial>(now, null);
                storages.Add(w);
                s = w.Item;
            }

            // s.Save(value);
        }

        private int binarySearch(List<WeightedWrapper<LinearPolynomial>> arr, int target, int l, int r)
        {
            if (l <= r)
            {
                int mid = l + (r - l) / 2;
                if (arr[mid].W == target) return mid;
                if (arr[mid].W > target) return binarySearch(arr, target, l, mid - 1);

                return binarySearch(arr, target, mid + 1, r);
            }

            return -1;
        }

        private int Search(List<WeightedWrapper<LinearPolynomial>> arr, int target)
        {
            return binarySearch(arr, target, 0, arr.Count - 1);
        }
    }

    class Fx<T> : IStorage<T> where T : IComparable
    {  
        
        public T Factor;
        public T Value;

        public Fx()
        {
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public T Load()
        {
            throw new NotImplementedException();
        }

        public void Save(T value)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

}