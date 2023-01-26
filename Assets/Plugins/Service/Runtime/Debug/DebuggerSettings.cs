using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class DebuggerSettings : ScriptableObject
    {
        public List<bool> flags;

        public DebuggerSettings()
        {
            flags = new List<bool>();
            int length = System.Enum.GetValues(typeof(EMessageType)).Length;
            for (int i = 0; i < length; i++)
            {
                flags.Add(false);
            }
        }

        public bool AllowLog(EMessageType type)
        {
            return flags[(int)type];
        }
    }
}