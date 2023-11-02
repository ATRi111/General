using UnityEngine;

namespace Services.Save
{
    /// <summary>
    /// 控制一个SaveDataGroup的读写
    /// </summary>
    public class SaveGroupController : MonoBehaviour
    {
        [SerializeField,HideInInspector]
        internal SaveDataGroup group;

        internal bool needLoad;
        [SerializeField]
        protected bool readOnAwake = true;

        [SerializeField]
        internal string fileName;
        protected string savePath;
        [SerializeField]
        internal int groupId;

        protected virtual void Awake()
        {
            savePath = SaveUtility.GenerateSavePath(fileName);
            if (readOnAwake)
                Read();
        }

        /// <summary>
        /// <para>获取指定标识符、指定类型的存档数据，如果没有，则创建一份；然后将其与指定对象绑定，并试图读档</para>
        /// </summary>
        public T Bind<T>(string identifier, Object obj) where T : SaveData, new()
        {
            T ret = group.Bind<T>(identifier, obj);
            if (ret != null && needLoad)
                ret.Load();
            return ret;
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
            group = SaveUtility.Read(savePath);
            group.Initialize();
        }

        public void Write()
        {
            SaveUtility.Write(savePath, group);
        }
    }
}