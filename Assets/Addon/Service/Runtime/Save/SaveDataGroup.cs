using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Services.Save
{
    /// <summary>
    /// <para>一组SaveData，具有相同的存档和读档时机的SaveData应当划分到一组中</para>
    /// </summary>
    [System.Serializable]
    public sealed class SaveDataGroup
    {
        internal Dictionary<string, SaveData> searcher;
        [JsonProperty]
        internal List<SaveData> dataList;

        internal SaveDataGroup()
        {
            searcher = new Dictionary<string, SaveData>();
            dataList = new List<SaveData>();
        }

        internal T Bind<T>(string identifier, Object obj) where T : SaveData, new()
        {
            if (!searcher.ContainsKey(identifier))
            {
                T t = new();
                searcher.Add(identifier, t);
                dataList.Add(t);
            }
            searcher[identifier].Initialize(identifier, obj);
            return searcher[identifier] as T;
        }

        internal void Initialize()
        {
            dataList ??= new List<SaveData>();
            searcher ??= new Dictionary<string, SaveData>();
            for (int i = 0; i < dataList.Count; i++)
            {
                searcher.Add(dataList[i].identifier, dataList[i]);
            }
        }

        internal void Load()
        {
            foreach (SaveData data in searcher.Values)
            {
                data.LoadIfExist();
            }
        }

        internal void Save()
        {
            foreach (SaveData data in searcher.Values)
            {
                data.SaveIfExist();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            for (int i = 0; i < dataList.Count; i++)
            {
                sb.AppendLine(dataList[i].ToString());
            }
            return sb.ToString();
        }
    }
}