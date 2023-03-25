using System;
using System.IO;
using UnityEngine;

namespace Services
{
    public static class JsonTool
    {
        /// <summary>
        /// ����Ϊjson
        /// </summary>
        /// <param name="path">·����Ҫ����չ��</param>
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
                Debugger.LogWarning($"дJsonʧ�ܣ�·��Ϊ{path}", EMessageType.System);
                Debugger.LogError(e.ToString(), EMessageType.System);
            }
        }

        /// <summary>
        /// ��ȡjson
        /// </summary>
        /// <param name="path">·����Ҫ����չ��</param>
        public static T LoadFromJson<T>(string path) where T : class
        {
            FileTool.GetFileInfo(path);
            try
            {
                return JsonUtility.FromJson<T>(File.ReadAllText(path));
            }
            catch (Exception e)
            {
                Debugger.LogWarning($"��ȡJsonʧ�ܣ�·��Ϊ{path}", EMessageType.System);
                Debugger.LogWarning(e.ToString(), EMessageType.System);
                return null;
            }
        }
    }
}