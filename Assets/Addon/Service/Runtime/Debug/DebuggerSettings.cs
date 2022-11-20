using UnityEngine;

namespace Services
{
    public class DebuggerSettings : ScriptableObject
    {
        public bool[] flags;
        
        public DebuggerSettings()
        {
            flags = new bool[System.Enum.GetValues(typeof(EMessageType)).Length];
            for (int i = 0; i < flags.Length; i++)
            {
                flags[i] = true;
            }
        }

        public bool AllowLog(EMessageType type)
        {
            return flags[(int)type];
        }
    }
}