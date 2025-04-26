using System.Collections.Generic;

namespace Algorithms
{
    public class TimeSet<T>
    {
        private Dictionary<T, float> dict = new Dictionary<T, float>();
        private Queue<(T key, float time)> timeQueue = new Queue<(T key, float time)>();
        private float currentTime;

        public void UpdateTime(float t)
        {
            currentTime = t;
            while (timeQueue.Count > 0 && timeQueue.Peek().time <= t)
            {
                var item = timeQueue.Dequeue();
                if (dict.TryGetValue(item.key, out float storedTime) && storedTime == item.time)
                {
                    dict.Remove(item.key);
                }
            }
        }

        public bool Contains(T key)
        {
            return dict.ContainsKey(key);
        }

        public void Insert(float t, T key)
        {
            if (t < currentTime)
                return;
                
            dict[key] = t;
            timeQueue.Enqueue((key, t));
        }

        public void Remove(T key)
        {
            dict.Remove(key);
        }
    }
}