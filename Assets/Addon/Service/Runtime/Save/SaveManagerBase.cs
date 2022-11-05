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
        /// ������Ϊ���������ɵĽű�ͨ���������ж��Ƿ���Ҫ����
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
        /// ������Ϊ����ǰ���ɵĽű�ͨ�����¼���������
        /// </summary>
        public event UnityAction AfterLoad;
        /// <summary>
        /// �浵�¼����浵ʱ��һЩ�����Լ������ݴ���RuntimeData,Ȼ��RuntimeDataд��浵�ļ�
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
                Debug.LogWarning("�޷���ȡ�浵�������´浵");
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
                Debug.LogWarning("�޷�д��浵");
            }
        }

        public void Save(string savePath)
        {
            Debug.Log("�浵");
            BeforeSave?.Invoke();
            Write(savePath);
        }
    }
}