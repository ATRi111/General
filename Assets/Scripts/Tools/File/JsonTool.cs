using System;
using System.IO;
using UnityEngine;

public static class JsonTool 
{
    /// <summary>
    /// 保存为json
    /// </summary>
    /// <param name="path">路径，要加拓展名</param>
    public static void SaveAsJson<T>(T t, string path)
    {
        try
        {
            FileInfo info = FileTool.GetFileInfo(path, true);
            using StreamWriter writer = new StreamWriter(path, false, FileTool.s_defaultEncoding);
            writer.WriteLine(JsonUtility.ToJson(t));
        }
        catch(Exception e)
        {
            Debug.LogWarning($"写Json失败，路径为{path}");
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// 读取json
    /// </summary>
    /// <param name="path">路径，要加拓展名</param>
    public static T LoadFromJson<T>(string path)
    {
        FileTool.GetFileInfo(path);
        try
        {
            return JsonUtility.FromJson<T>(File.ReadAllText(path));
        }
        catch (Exception e)
        {
            Debug.LogWarning($"读取Json失败，路径为{path}");
            Debug.LogError(e);
            return default;
        }
    }
}
