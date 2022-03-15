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
    /// <param name="path">路径，要包含拓展名</param>
    /// <param name="create">文件不存在时，是否新创建文件</param>
    public static FileInfo GetFileInfo(string path, bool create = false)
    {
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
        {
            if (create)
                fileInfo.Create().Dispose();
            else
                Debug.LogWarning($"{path}文件不存在");
        }
        return fileInfo;
    }
}
