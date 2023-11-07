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
        internal Dictionary<string, SaveData> runtimeDatas;
        [JsonProperty]
        internal List<SaveData> datas;

        internal SaveDataGroup()
        {
            runtimeDatas = new Dictionary<string, SaveData>();
            datas = new List<SaveData>();
        }

        internal T Bind<T>(string identifier, Object obj) where T : SaveData, new()
        {
            if (!runtimeDatas.ContainsKey(identifier))
            {
                T t = new();
                runtimeDatas.Add(identifier, t);
                datas.Add(t);
            }
            runtimeDatas[identifier].Initialize(identifier, obj);
            return runtimeDatas[identifier] as T;
        }

        internal void Initialize()
        {
            datas ??= new List<SaveData>();
            runtimeDatas ??= new Dictionary<string, SaveData>();
            for (int i = 0; i < datas.Count; i++)
            {
                runtimeDatas.Add(datas[i].identifier, datas[i]);
            }
        }

        internal void Load()
        {
            foreach (SaveData data in runtimeDatas.Values)
            {
                data.LoadIfExist();
            }
        }

        internal void Save()
        {
            foreach (SaveData data in runtimeDatas.Values)
            {
                data.SaveIfExist();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < datas.Count; i++)
            {
                sb.AppendLine(datas[i].ToString());
            }
            return sb.ToString();
        }
    }
}