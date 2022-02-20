using System.IO;
using System.Text;
using UnityEngine;

public static class FileTool
{
    public static Encoding s_defaultEncoding = Encoding.UTF8;

    /// <summary>
    /// ��ȡ�ļ���Ϣ
    /// </summary>
    /// <param name="path">·��</param>
    /// <param name="defaultPath">�Ƿ�ʹ��Ĭ��·����Application.streamingAssetsPath��</param>
    public static FileInfo GetFileInfo(string path, bool defaultPath = true)
    {
        string combine = defaultPath ? Path.Combine(Application.streamingAssetsPath, path) : path;
        FileInfo fileInfo = new FileInfo(combine);
        if (!fileInfo.Exists)
        {
            Debug.LogWarning($"{path}�ļ�������");
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
            Debug.LogWarning($"�޷�����{path}�ļ�");
            return null;
        }
    }
}
