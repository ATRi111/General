using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// Ҫ���������¼���ʱ���
    /// </summary>
    public enum EInputDuration
    {
        /// <summary>
        /// ��ǰ
        /// </summary>
        Now,
        /// <summary>
        /// ����һ��Update���������ڣ����ڽ��е�Update��Ӱ��������ĳ�����¼��Ƿ���
        /// </summary>
        FromUpdate,
        /// <summary>
        /// ����һ��FixedUpdate���������ڣ����ڽ��е�FixedUpdate��Ӱ��������ĳ�����¼��Ƿ���
        /// </summary>
        FromFixedUpdate,
    }

    /// <summary>
    /// ��¼���롣���Ҫ��ȡ���µ����룬ֱ�ӵ���InputManager�е�API����;���ౣ����ǹ�ȥ�������Լ��޷�ֱ�ӻ�ȡ������
    /// </summary>
    public static class InputRecoder 
    {
        private readonly static Dictionary<string, RecordItem> items;

        public static bool GetButtonUp(string axisName,EInputDuration duration)
        {
            RecordItem item = items[axisName];
            return duration switch
            {
                EInputDuration.FromUpdate => item.up_update,
                EInputDuration.FromFixedUpdate => item.up_fixedUpdate,
                EInputDuration.Now => Input.GetButtonUp(axisName),
                _ => false,
            };
        }

        public static bool GetButtonDown(string axisName,EInputDuration duration)
        {
            RecordItem item = items[axisName];
            return duration switch
            {
                EInputDuration.FromUpdate => item.down_update,
                EInputDuration.FromFixedUpdate => item.down_fixedUpdate,
                EInputDuration.Now => Input.GetButtonDown(axisName),
                _ => false,
            };
        }

        public static void AddItem(string axisName)
        {
            if(!items.ContainsKey(axisName))
                items.Add(axisName, new RecordItem());
        }

        public static void RemoveItem(string axisName)
        {
            items.Remove(axisName);
        }

        static InputRecoder()
        {
            items = new Dictionary<string, RecordItem>();
            GameObject obj = new GameObject(nameof(InputRecoderMono));
            obj.AddComponent<InputRecoderMono>();
            Object.DontDestroyOnLoad(obj);
        }

        internal static void AfterUpdate()
        {
            foreach (RecordItem item in items.Values)
            {
                item.AfterUpdate();
            }
        }

        internal static void AfterFixedUpdate()
        {
            foreach (RecordItem item in items.Values)
            {
                item.AfterFixedUpdate();
            }
        }
    }
}