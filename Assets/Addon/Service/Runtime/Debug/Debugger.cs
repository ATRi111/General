using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    public static class Debugger
    {
        public static DebuggerSettings settings;

        static Debugger()
        {
            settings = Resources.Load<DebuggerSettings>("DebuggerSettings");
        }

        public static void Log(string message, EMessageType type = EMessageType.Default)
        {
            Log(message, type, Debug.Log);
        }

        public static void LogWarning(string message, EMessageType type = EMessageType.Default)
        {
            Log(message, type, Debug.LogWarning);
        }

        public static void LogError(string message, EMessageType type = EMessageType.Default)
        {
            Log(message, type, Debug.LogError);
        }

        private static void Log(string message, EMessageType type, UnityAction<string> method)
        {
            if(settings.AllowLog(type))
            {
                message = FormatMessage(message, type);
                method.Invoke(message);
            }
        }

        public static string FormatMessage(string message, EMessageType type)
        {
            return $"(<b>{type}</b>): {message}";
        }
    }
}