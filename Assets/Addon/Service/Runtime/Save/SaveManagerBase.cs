using System;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    public class SaveManagerBase : Service
    {
        protected EventSystem eventSystem;

        protected bool needLoad;
        /// <summary>
        /// 是否需要读档
        /// </summary>
        public bool NeedLoad
        {
            get => needLoad;
            set
            {
                needLoad = value;
                if (value)
                    LoadRequest?.Invoke();
            }
        }
        /// <summary>
        /// 发出读档请求后，一些对象从RuntimeData读取数据(读取的是当前的RuntimeData,如果要直接读取硬盘上的数据，需要先调用Read)
        /// </summary>
        public UnityAction LoadRequest;
        /// <summary>
        /// 发出存档请求后，一些对象将自己的数据传给RuntimeData(只是修改RuntimeData，如果要写到硬盘上，需要再调用Save)
        /// </summary>
        public UnityAction SaveRequest;

        [SerializeField]
        protected SaveManagerCore core;
        public WholeSaveData RuntimeData => core.RuntimeData;

        protected void Read(string savePath)
        {
            try
            {
                core.Read(savePath);
            }
            catch (Exception e)
            {
                Debugger.LogWarning(e.ToString(), EMessageType.System);
                Debugger.LogWarning("无法读取存档，创建新存档", EMessageType.System);
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
                Debugger.LogWarning(e.ToString(), EMessageType.System);
                Debugger.LogWarning("无法写入存档", EMessageType.System);
            }
        }
    }
}