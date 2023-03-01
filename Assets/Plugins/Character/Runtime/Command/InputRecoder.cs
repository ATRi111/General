using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// 要检测的输入事件的时间段
    /// </summary>
    public enum EInputDuration
    {
        /// <summary>
        /// 当前
        /// </summary>
        Now,
        /// <summary>
        /// 从上一次Update结束后到现在（正在进行的Update不影响结果），某输入事件是否发生
        /// </summary>
        FromUpdate,
        /// <summary>
        /// 从上一次FixedUpdate结束后到现在（正在进行的FixedUpdate不影响结果），某输入事件是否发生
        /// </summary>
        FromFixedUpdate,
    }

    /// <summary>
    /// 记录输入。如果要获取当下的输入，直接调用InputManager中的API即可;此类保存的是过去的输入以及无法直接获取的输入
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