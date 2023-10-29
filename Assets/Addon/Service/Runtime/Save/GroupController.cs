using System;
using UnityEngine;

namespace Services.Save
{
    /// <summary>
    /// 控制一个SaveDataGroup的读写
    /// </summary>
    public class GroupController : MonoBehaviour
    {
        [SerializeField]
        internal SaveDataGroup group;

        internal bool needLoad;

        [SerializeField]
        internal string savePath;
        [SerializeField]
        internal int groupId;

        /// <summary>
        /// <para>获取指定标识符、指定类型的存档数据，如果没有，则创建一份；然后将其与指定对象绑定，并试图读档</para>
        /// </summary>
        public T Bind<T>(string identifier, UnityEngine.Object obj) where T : SaveData, new()
        {
            T ret = group.Bind<T>(identifier, obj);
            if (ret != null && needLoad)
                ret.OnLoad();
            return ret;
        }

        public void Load()
        {
            Read();
            needLoad = true;
        }

        public void ResetLoad()
        {
            needLoad = false;
        }

        public void Save()
        {
            group.OnSave();
            Write();
        }

        public void Read()
        {
            try
            {
                group = SaveUtility.Read(savePath);
            }
            catch (Exception e)
            {
                Debugger.LogWarning(e.ToString(), EMessageType.System);
                Debugger.LogWarning("无法读取存档，创建新存档", EMessageType.System);
            }
        }

        public void Write()
        {
            try
            {
                SaveUtility.Write(savePath, group);
            }
            catch (Exception e)
            {
                Debugger.LogWarning(e.ToString(), EMessageType.System);
                Debugger.LogWarning("无法写入存档", EMessageType.System);
            }
        }
    }
}