using System;
using System.Collections.Generic;
using Seyren.Algorithms;
using UnityEngine;

namespace Seyren.Unity
{
    public static class ObjectPool
    {
        public static Dictionary<string, int> PoolSize = new Dictionary<string, int>();
        static Dictionary<string, Pool<GameObject>> pool = new Dictionary<string, Pool<GameObject>>();

        static Func<GameObject> Creator(string path)
        {
            return () =>
            {
                GameObject cover = new GameObject();
                GameObject blueprint = Resources.Load<GameObject>(path);
                GameObject obj = GameObject.Instantiate(blueprint);
                obj.transform.parent = cover.transform;
                cover.SetActive(false);

                return cover;
            };
        }

        public static GameObject GetObject(string path)
        {
            if (!pool.ContainsKey(path))
            {
                int size = PoolSize.ContainsKey(path) ? PoolSize[path] : 10;
                pool.Add(path, new Pool<GameObject>(size, Creator(path)));
            }

            GameObject obj = pool[path].Get();
            obj.SetActive(true);
            return obj;
        }

        public static void ReturnObject(string path, GameObject obj)
        {
            if (!pool.ContainsKey(path))
            {
                return;
            }

            obj.SetActive(false);
            pool[path].Return(obj);
        }
    }

}