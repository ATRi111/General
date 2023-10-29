using System.Collections.Generic;
using UnityEngine;

namespace Services.Save
{
    /// <summary>
    /// <para>一组SaveData，具有相同的存档和读档时机的SaveData应当划分到一组中</para>
    /// </summary>
    [System.Serializable]
    public class SaveDataGroup
    {
        internal Dictionary<string, SaveData> runtimeDatas;
        [SerializeField]
        internal List<SaveData> datas;

        internal SaveDataGroup()
        {
            runtimeDatas = new Dictionary<string, SaveData>();
            datas = new List<SaveData>();
        }

        internal T Bind<T>(string identifier,Object obj) where T : SaveData,new()
        {
            if(!runtimeDatas.ContainsKey(identifier))
            {
                T t = new();
                runtimeDatas.Add(identifier, t);
                datas.Add(t);
            }
            runtimeDatas[identifier].obj = obj;
            return runtimeDatas[identifier] as T;
        }

        internal void AfterRead()
        {
            for (int i = 0; i < datas.Count; i++)
            {
                runtimeDatas.Add(datas[i].Identifier, datas[i]);
            }
        }

        internal void OnSave() 
        {
            foreach (SaveData data in runtimeDatas.Values)
            {
                data.OnSave();
            }
        }
    }
}