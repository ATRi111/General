using System;
using System.Collections.Generic;

namespace Tools
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

        protected Dictionary<X, Y> injecion1;
        protected Dictionary<Y, X> injecion2;

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
            injecion1 = new Dictionary<X, Y>();
            injecion2 = new Dictionary<Y, X>();
        }
        public Bijection(Dictionary<X, Y> injecion)
        {
            injecion1 = injecion;
            injecion2 = CreateInjection(injecion);
        }
        public Bijection(Dictionary<Y, X> injecion)
        {
            injecion2 = injecion;
            injecion1 = CreateInjection(injecion);
        }

        public void Add(X x, Y y)
        {
            if (injecion1.ContainsKey(x) || injecion2.ContainsKey(y))
                throw new ArgumentException();
            injecion1.Add(x, y);
            injecion2.Add(y, x);
        }

        public Y Get(X key)
        {
            if (!injecion1.ContainsKey(key))
            {
                UnityEngine.Debug.LogWarning($"不包含{key}元素");
                return default;
            }
            return injecion1[key];
        }
        public X Get(Y key)
        {
            if (!injecion2.ContainsKey(key))
            {
                UnityEngine.Debug.LogWarning($"不包含{key}元素");
                return default;
            }
            return injecion2[key];
        }

        public void Modify(X key, Y value)
        {
            if (!injecion1.ContainsKey(key))
                throw new IndexOutOfRangeException();
            Y origin = injecion1[key];
            injecion1[key] = value;
            injecion2.Remove(origin);
            injecion2.Add(value, key);
        }
        public void Modify(Y key, X value)
        {
            if (!injecion2.ContainsKey(key))
                throw new IndexOutOfRangeException();
            X origin = injecion2[key];
            injecion2[key] = value;
            injecion1.Remove(origin);
            injecion1.Add(value, key);
        }

        public void Remove(X x)
        {
            if (injecion1.ContainsKey(x))
            {
                Y y = injecion1[x];
                injecion1.Remove(x);
                injecion2.Remove(y);
            }
        }
        public void Remove(Y y)
        {
            if (injecion2.ContainsKey(y))
            {
                X x = injecion2[y];
                injecion1.Remove(x);
                injecion2.Remove(y);
            }
        }

        public void Clear()
        {
            injecion1.Clear();
            injecion2.Clear();
        }
    }
}