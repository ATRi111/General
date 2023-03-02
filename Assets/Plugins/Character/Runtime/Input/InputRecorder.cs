using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// 检测输入事件的时间段
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
    /// 保存的过去的输入,包括无法直接获取的输入
    /// </summary>
    public static class InputRecorder 
    {
        private readonly static Dictionary<string, RecordItem> items;

        public static bool GetButtonUp(string axisName,EInputDuration duration)
        {
            if (!items.ContainsKey(axisName))
                AddItem(axisName);
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
            if (!items.ContainsKey(axisName))
                AddItem(axisName);
            RecordItem item = items[axisName];
            return duration switch
            {
                EInputDuration.FromUpdate => item.down_update,
                EInputDuration.FromFixedUpdate => item.down_fixedUpdate,
                EInputDuration.Now => Input.GetButtonDown(axisName),
                _ => false,
            };
        }

        /// <summary>
        /// 添加对一个轴的输入检测（访问一个轴时，如果尚未添加，则会自动添加；但未事先添加可能导致第一次获取到错误的结果）
        /// </summary>
        public static void AddItem(string axisName)
        {
            if (!items.ContainsKey(axisName))
                items.Add(axisName, new RecordItem(axisName));
        }

        /// <summary>
        /// 取消对一个轴的输入检测
        /// </summary>
        public static void RemoveItem(string axisName)
        {
            items.Remove(axisName);
        }

        static InputRecorder()
        {
            items = new Dictionary<string, RecordItem>();
            GameObject obj = new GameObject(nameof(InputRecorderMono));
            obj.AddComponent<InputRecorderMono>();
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