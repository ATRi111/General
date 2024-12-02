using System;
using System.Collections.Generic;

namespace MyTool
{
    /// <summary>
    /// 双向字典
    /// </summary>
    public class Bijection<X, Y>
    {
        public static Dictionary<T1, T2> CreateInjection<T1, T2>(Dictionary<T2, T1> injection)
        {
            Dictionary<T1, T2> ret = new Dictionary<T1, T2>();
            foreach (KeyValuePair<T2, T1> pair in injection)
            {
                ret.Add(pair.Value, pair.Key);
            }
            return ret;
        }

        protected Dictionary<X, Y> injection1;
        protected Dictionary<Y, X> injection2;

        public Y this[X x]
        {
            get => Get(x);
            set => Modify(x, value);
        }

        public X this[Y y]
        {
            get => Get(y);
            set => Modify(y, value);
        }

        public Bijection()
        {
            injection1 = new Dictionary<X, Y>();
            injection2 = new Dictionary<Y, X>();
        }
        public Bijection(Dictionary<X, Y> injection)
        {
            injection1 = injection;
            injection2 = CreateInjection(injection);
        }
        public Bijection(Dictionary<Y, X> injection)
        {
            injection2 = injection;
            injection1 = CreateInjection(injection);
        }

        public void Add(X x, Y y)
        {
            if (injection1.ContainsKey(x) || injection2.ContainsKey(y))
                throw new ArgumentException();
            injection1.Add(x, y);
            injection2.Add(y, x);
        }

        public Y Get(X key)
        {
            if (!injection1.ContainsKey(key))
            {
                throw new ArgumentException();
            }
            return injection1[key];
        }
        public X Get(Y key)
        {
            if (!injection2.ContainsKey(key))
            {
                UnityEngine.Debug.LogWarning($"不包含{key}元素");
                return default;
            }
            return injection2[key];
        }

        public void Modify(X key, Y value)
        {
            if (!injection1.ContainsKey(key))
                throw new IndexOutOfRangeException();
            Y origin = injection1[key];
            injection1[key] = value;
            injection2.Remove(origin);
            injection2.Add(value, key);
        }
        public void Modify(Y key, X value)
        {
            if (!injection2.ContainsKey(key))
                throw new IndexOutOfRangeException();
            X origin = injection2[key];
            injection2[key] = value;
            injection1.Remove(origin);
            injection1.Add(value, key);
        }

        public void Remove(X x)
        {
            if (injection1.ContainsKey(x))
            {
                Y y = injection1[x];
                injection1.Remove(x);
                injection2.Remove(y);
            }
        }
        public void Remove(Y y)
        {
            if (injection2.ContainsKey(y))
            {
                X x = injection2[y];
                injection1.Remove(x);
                injection2.Remove(y);
            }
        }

        public void Clear()
        {
            injection1.Clear();
            injection2.Clear();
        }
    }
}