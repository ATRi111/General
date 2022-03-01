using System.IO;
using System.Text;
using UnityEngine;

public static class FileTool
{
    public static Encoding s_defaultEncoding = Encoding.UTF8;

    /// <summary>
    /// ��pathǰ����Application.streamingAssetsPath
    /// </summary>
    public static string AsDefualtPath(this string path)
    {
        return Path.Combine(Application.streamingAssetsPath, path);
    }
    /// <summary>
    /// ��ȡ�ļ���Ϣ
    /// </summary>
    /// <param name="path">·��</param>
    /// <param name="defaultPath">�Ƿ�ʹ��Ĭ��·����Application.streamingAssetsPath��</param>
    public static FileInfo GetFileInfo(string path)
    {
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
        {
            Debug.LogWarning($"�ļ������ڣ�·��Ϊ{path}");
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
            Debug.LogWarning($"�޷���ȡStreamReader,·��Ϊ{path}");
            return null;
        }
    }
}
