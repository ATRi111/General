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
        /// �Ƿ���Ҫ����
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
        /// �������������һЩ�����RuntimeData��ȡ����(��ȡ���ǵ�ǰ��RuntimeData,���Ҫֱ�Ӷ�ȡӲ���ϵ����ݣ���Ҫ�ȵ���Read)
        /// </summary>
        public UnityAction LoadRequest;
        /// <summary>
        /// �����浵�����һЩ�����Լ������ݴ���RuntimeData(ֻ���޸�RuntimeData�����Ҫд��Ӳ���ϣ���Ҫ�ٵ���Save)
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
                Debugger.LogWarning("�޷���ȡ�浵�������´浵", EMessageType.System);
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
                Debugger.LogWarning("�޷�д��浵", EMessageType.System);
            }
        }
    }
}