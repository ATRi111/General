using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTool
{
    public abstract class SerializedKeyValueBase
    {

    }

    [Serializable]
    public sealed class SerializedKeyValuePair<TKey, TValue> : SerializedKeyValueBase
    {
        [SerializeField]
        private TKey key;
        public TKey Key => key;
        [SerializeField]
        private TValue value;
        public TValue Value => value;

        public SerializedKeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }

        public static explicit operator KeyValuePair<TKey, TValue>(SerializedKeyValuePair<TKey, TValue> pair)
        {
            return new KeyValuePair<TKey, TValue>(pair.Key, pair.Value);
        }

        public static explicit operator SerializedKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> pair)
        {
            return new SerializedKeyValuePair<TKey, TValue>(pair.Key, pair.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(key, value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is not SerializedKeyValuePair<TKey, TValue> pair)
                return false;
            return key.Equals(pair.key) && value.Equals(pair.value);
        }
        public override string ToString()
        {
            return $"Key: {key}, Value: {value}";
        }
    }

    public class SerializedDictionaryBase
    {

    }

    [Serializable]
    public class SerializedDictionary<TKey, TValue> : SerializedDictionaryBase, IDictionary<TKey, TValue>
    {
        protected readonly Dictionary<TKey, TValue> dict;
        [SerializeField]
        protected List<SerializedKeyValuePair<TKey, TValue>> list;

        public int Count => list.Count;

        public bool IsReadOnly => false;

        public ICollection<TKey> Keys => dict.Keys;

        public ICollection<TValue> Values => dict.Values;

        public TValue this[TKey key]
        {
            get => dict[key];
            set
            {
                dict[key] = value;
                int i = list.IndexOf(new SerializedKeyValuePair<TKey, TValue>(key, value));
                list[i] = new SerializedKeyValuePair<TKey, TValue>(key, value);
            }
        }

        public SerializedDictionary()
        {
            dict = new();
        }

        public void Refresh()
        {
            dict.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                dict.Add(list[i].Key, list[i].Value);
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (!ContainsKey(key))
            {
                dict.Add(key, value);
                list.Add(new SerializedKeyValuePair<TKey, TValue>(key, value));
            }
        }

        public bool ContainsKey(TKey key)
        {
            return dict.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            if (TryGetValue(key, out TValue value))
            {
                list.Remove(new SerializedKeyValuePair<TKey, TValue>(key, value));
                dict.Remove(key);
                return true;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return dict.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            dict.Clear();
            list.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return dict.ContainsKey(item.Key) && dict[item.Key].Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            for (int i = 0; i < list.Count; i++)
            {
                array[arrayIndex] = (KeyValuePair<TKey, TValue>)list[i];
                arrayIndex++;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}