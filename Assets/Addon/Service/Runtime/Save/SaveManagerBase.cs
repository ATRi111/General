using System;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    public class SaveManagerBase : Service
    {
        [Other]
        protected EventSystem eventSystem;

        protected bool needLoad;
        /// <summary>
        /// 读档行为发生后生成的脚本通过此属性判断是否需要读档
        /// </summary>
        public bool NeedLoad 
        { 
            get => needLoad;
            set
            {
                needLoad = value;
                if(value)
                    AfterLoad?.Invoke();
            }
        }
        /// <summary>
        /// 读档行为发生前生成的脚本通过此事件监听读档
        /// </summary>
        public event UnityAction AfterLoad;
        /// <summary>
        /// 存档事件，存档时，一些对象将自己的数据传给RuntimeData,然后将RuntimeData写入存档文件
        /// </summary>
        public event UnityAction BeforeSave;

        protected SaveManagerCore core;
        public WholeSaveData RuntimeData => core.RuntimeData;

        protected void Read(string savePath)
        {
            try
            {
                core.Read(savePath);
            }
            catch(Exception e)
            {
                Debug.LogWarning(e);
                Debug.LogWarning("无法读取存档，创建新存档");
            }
        }

        protected void Write(string savePath)
        {
            try
            {
                core.Write(savePath);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                Debug.LogWarning("无法写入存档");
            }
        }

        public void Save(string savePath)
        {
            Debug.Log("存档");
            BeforeSave?.Invoke();
            Write(savePath);
        }
    }
}