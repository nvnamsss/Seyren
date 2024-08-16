using System;
using System.Collections.Generic;
using System.Drawing;

namespace Seyren.Algorithms {
    public class Pool<T> {
        public int Size => size;
        Func<T> creator;

        Queue<T> queue;
        int size;
        public Pool(int size, Func<T> creator) {
            this.creator = creator;
            this.size = size;
            queue = new Queue<T>();

            for (int i = 0; i < size; i++) {
                queue.Enqueue(creator());
            }
        }

        public T Get() {
            if (queue.Count > 0) {
                return queue.Dequeue();
            }
            return creator();
        }

        public bool Return(T obj) {
            queue.Enqueue(obj);

            return true;
        }
    }
}