using System.IO;
using System.Text;
using UnityEngine;

public static class FileTool
{
    public static Encoding s_defaultEncoding = Encoding.UTF8;

    /// <summary>
    /// 获取文件信息
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="defaultPath">是否使用默认路径（Application.streamingAssetsPath）</param>
    public static FileInfo GetFileInfo(string path, bool defaultPath = true)
    {
        string combine = defaultPath ? Path.Combine(Application.streamingAssetsPath, path) : path;
        FileInfo fileInfo = new FileInfo(combine);
        if (!fileInfo.Exists)
        {
            Debug.LogWarning($"{path}文件不存在");
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
            Debug.LogWarning($"无法加载{path}文件");
            return null;
        }
    }
}
