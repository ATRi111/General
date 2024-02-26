using System;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    public static class Debugger
    {
        public static DebuggerSettings Settings
        {
            get
            {
                if(settings == null)
                {
                    try
                    {
                        settings = Resources.Load<DebuggerSettingSO>("DebuggerSettings").settings;
                        Settings.Copy();
                    }
                    catch
                    {
                        return new DebuggerSettings();
                    }
                }
                return settings;
            }
        }
        private static DebuggerSettings settings;
        public static void Log(object message, EMessageType type = EMessageType.Default)
        {
            Log(message, type, Debug.Log);
        }

        public static void LogWarning(object message, EMessageType type = EMessageType.Default)
        {
            Log(message, type, Debug.LogWarning);
        }

        public static void LogError(object message, EMessageType type = EMessageType.Default)
        {
            Log(message, type, Debug.LogError);
        }

        private static void Log(object message, EMessageType type, UnityAction<object> method)
        {
            if (Settings.GetAllowLog(type))
            {
                message = FormatMessage(message, type);
                method.Invoke(message);
            }
        }

        public static string FormatMessage(object message, EMessageType type)
        {
            return $"(<b>{type}</b>): {message}";
        }

        public static void LogException(Exception e, EMessageType type = EMessageType.Default)
        {
            if (Settings.GetAllowLog(type))
            {
                Debug.LogException(e);
            }
        }
    }
}