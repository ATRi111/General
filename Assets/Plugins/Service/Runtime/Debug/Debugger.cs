using System;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    public static class Debugger
    {
        public static DebuggerSettings settings;

        static Debugger()
        {
            settings = Resources.Load<DebuggerSettings>(nameof(DebuggerSettings));
        }

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
            if (settings.AllowLog(type))
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
            if (settings.AllowLog(type))
            {
                Debug.LogException(e);
            }
        }
    }
}