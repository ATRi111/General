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
    /// ��ȡ�ļ���Ϣ
    /// </summary>
    /// <param name="path">·����Ҫ������չ��</param>
    /// <param name="create">�ļ�������ʱ���Ƿ��´����ļ�</param>
    public static FileInfo GetFileInfo(string path, bool create = false)
    {
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
        {
            if (create)
                fileInfo.Create().Dispose();
            else
                Debug.LogWarning($"{path}�ļ�������");
        }
        return fileInfo;
    }
}
