using LitJson;
using System;
using System.IO;
using UnityEngine;

public static class JsonTool 
{
    /// <summary>
    /// ����Ϊjson
    /// </summary>
    /// <param name="path">·����Ҫ����չ��</param>
    public static void SaveAsJson<T>(T t, string path)
    {
        try
        {
            FileInfo info = FileTool.GetFileInfo(path, true);
            using StreamWriter writer = new StreamWriter(path, false, FileTool.s_defaultEncoding);
            writer.WriteLine(JsonMapper.ToJson(t));
        }
        catch(Exception e)
        {
            Debug.LogWarning($"дJsonʧ�ܣ�·��Ϊ{path}");
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// ��ȡjson
    /// </summary>
    /// <param name="path">·����Ҫ����չ��</param>
    public static T LoadFromJson<T>(string path)
    {
        FileInfo info = FileTool.GetFileInfo(path);
        try
        {
            return JsonMapper.ToObject<T>(File.ReadAllText(path));
        }
        catch
        {
            Debug.LogWarning($"��ȡJsonʧ�ܣ�·��Ϊ{path}");
            return default;
        }
    }
}
