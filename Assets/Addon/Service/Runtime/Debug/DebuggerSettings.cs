using System.Collections.Generic;

namespace Services
{
    [System.Serializable]
    public class DebuggerSettings
    {
        public List<bool> flags;
        private readonly List<bool> copy = new List<bool>();

        public DebuggerSettings()
        {
            flags = new List<bool>();
            int length = System.Enum.GetValues(typeof(EMessageType)).Length;
            for (int i = 0; i < length; i++)
            {
                flags.Add(true);
            }
        }

        public bool GetAllowLog(EMessageType type)
        {
            return flags[(int)type];
        }

        public void SetAllowLog(EMessageType type, bool value)
        {
            flags[(int)type] = value;
        }

        /// <summary>
        /// 生成一份当前设置的副本（副本仅用于编辑器模式运行时）
        /// </summary>
        public void Copy()
        {
            copy.Clear();
            copy.AddRange(flags);
        }

        /// <summary>
        /// 用副本覆盖当前设置（副本仅用于编辑器模式运行时）
        /// </summary>
        public void Paste()
        {
            flags.Clear();
            flags.AddRange(copy);
        }
    }
}