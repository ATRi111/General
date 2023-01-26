using Services.Event;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Services.Save
{
    public class SaveManagerBase : Service, ISaveManager
    {
        protected IEventSystem eventSystem;

        protected bool needLoad;
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
        public UnityEvent LoadRequest => _LoadRequest;
        public UnityEvent SaveRequest => _SaveRequest;

        private UnityEvent _LoadRequest = new UnityEvent();
        private UnityEvent _SaveRequest = new UnityEvent();


        public WholeSaveData RuntimeData => core.RuntimeData;

        [SerializeField]
        protected SaveManagerCore core;

        public void Read(string savePath)
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

        public void Write(string savePath)
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