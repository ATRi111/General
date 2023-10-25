using System.Collections.Generic;
using UnityEngine;

namespace Services.Save
{
    /// <summary>
    /// 一种对象的数据集合
    /// </summary>
    [System.Serializable]
    public class SaveData<T> where T : SingleSaveData, new()
    {
        protected Dictionary<string, T> searcher;

        [SerializeField]
        protected List<T> datas;

        public SaveData()
        {
            datas = new List<T>();
            searcher = new Dictionary<string, T>();
        }

        public T Get(string identifier)
        {
            if (searcher.ContainsKey(identifier))
                return searcher[identifier];
            else
            {
                T data = new T();
                searcher.Add(identifier, data);
                return data;
            }
        }

        public void Add(T data)
        {
            datas.Add(data);
            searcher.Add(data.identifier, data);
        }
    }
}