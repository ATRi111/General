using UnityEngine;

namespace Services.Save
{
    /// <summary>
    /// 控制一个SaveDataGroup的读写
    /// </summary>
    public class SaveGroupController : MonoBehaviour
    {
        internal SaveDataGroup group;

        internal bool needLoad;
        [SerializeField]
        protected bool readOnAwake = true;

        [SerializeField]
        protected internal string fileName, index;
        protected virtual string SavePath => SaveUtility.GenerateSavePath(fileName);

        [SerializeField]
        internal int groupId;

        protected virtual void Awake()
        {
            if (readOnAwake)
                Read();
        }

        /// <summary>
        /// <para>获取指定标识符、指定类型的存档数据，如果没有，则创建一份；然后将其与指定对象绑定，并试图读档</para>
        /// </summary>
        public T Bind<T>(string identifier, Object obj) where T : SaveData, new()
        {
            if(obj == null)
            {
                Debugger.LogWarning($"试图将空对象与存档数据绑定,对象名为{obj.name}", EMessageType.Save);
                return null;
            }
            T ret = group.Bind<T>(identifier, obj);
            if (ret != null && needLoad)
                ret.Load();
            return ret;
        }

        /// <summary>
        /// 设置接下来的存档槽位
        /// </summary>
        public void SetIndex(string index)
        {
            this.index = index;
        }

        public void Load()
        {
            group.Load();
            needLoad = true;
        }

        public void Save()
        {
            group.Save();
            Write();
        }

        public void Read()
        {
            group = SaveUtility.Read(SavePath);
            group.Initialize();
        }

        public void Write()
        {
            SaveUtility.Write(SavePath, group);
        }
    }
}