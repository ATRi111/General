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
                Debug.LogWarning($"дJsonʧ�ܣ�·��Ϊ{path}");
                Debug.LogError(e);
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
                Debug.LogWarning($"��ȡJsonʧ�ܣ�·��Ϊ{path}");
                Debug.LogWarning(e);
                return null;
            }
        }
    }
}