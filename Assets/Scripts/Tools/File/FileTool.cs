using System.IO;
using System.Text;
using UnityEngine;

public static class FileTool
{
    public static Encoding s_defaultEncoding = Encoding.UTF8;

    public static string StreamingAssetsPath(string fileName)
    {
        return Path.Combine(Application.streamingAssetsPath, fileName);
    }
    public static string PersistentDataPath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    /// <summary>
    /// 获取文件信息
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="defaultPath">是否使用默认路径（Application.streamingAssetsPath）</param>
    public static FileInfo GetFileInfo(string path)
    {
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
        {
            Debug.LogWarning($"文件不存在，路径为{path}");
            return null;
        }
        return fileInfo;
    }

    public static StreamReader GetStreamReader(string path)
    {
        FileInfo fileInfo = GetFileInfo(path);
        try
        {
            return new StreamReader(fileInfo.FullName, s_defaultEncoding);
        }
        catch
        {
            Debug.LogWarning($"无法获取StreamReader,路径为{path}");
            return null;
        }
    }

    public static StreamWriter GetStreamWriter(string path)
    {
        try
        {
            return new StreamWriter(path);
        }
        catch
        {
            Debug.LogWarning($"无法创建StreamWriter,路径为{path}");
            return null;
        }
    }
}
