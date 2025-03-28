using System.Collections.Generic;

namespace MyTool
{
    [System.Serializable]
    public class AutoDictionary<TKey, TValue>
    {
        public readonly Dictionary<TKey, TValue> dict = new();

        public TValue this[TKey key]
        {
            get
            {
                Check(key);
                return dict[key];
            }
            set
            {
                Check(key);
                dict[key] = value;
            }
        }

        protected void Check(TKey key)
        {
            if (!dict.ContainsKey(key))
                dict.Add(key, default);
        }

        public void Clear()
        {
            dict.Clear();
        }

    }

    [System.Serializable]
    public class CounterDictionary : AutoDictionary<string, int>
    {
        
    }
}