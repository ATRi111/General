using System;
using System.IO;
using UnityEngine;

namespace Services
{
    public static class JsonTool
    {
        /// <summary>
        /// 保存为json
        /// </summary>
        /// <param name="path">路径，要加拓展名</param>
        public static void SaveAsJson<T>(T t, string path, bool append = false)
        {
            try
            {
                FileInfo info = FileTool.GetFileInfo(path, true);
                using StreamWriter writer = new StreamWriter(path, append, FileTool.DefaultEncoding);
                writer.WriteLine(JsonUtility.ToJson(t, true));
            }
            catch (Exception e)
            {
                Debugger.LogWarning($"写Json失败，路径为{path}", EMessageType.System);
                Debugger.LogError(e.ToString(), EMessageType.System);
            }
        }

        /// <summary>
        /// 读取json
        /// </summary>
        /// <param name="path">路径，要加拓展名</param>
        public static T LoadFromJson<T>(string path) where T : class
        {
            FileTool.GetFileInfo(path);
            try
            {
                return JsonUtility.FromJson<T>(File.ReadAllText(path));
            }
            catch (Exception e)
            {
                Debugger.LogWarning($"读取Json失败，路径为{path}", EMessageType.System);
                Debugger.LogWarning(e.ToString(), EMessageType.System);
                return null;
            }
        }
    }
}