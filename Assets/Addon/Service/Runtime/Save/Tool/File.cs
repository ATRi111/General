using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static partial class FileTool
{
    public static Encoding DefaultEncoding = Encoding.UTF8;

    public static string CombinePath(params string[] paths)
    {
        StringBuilder sb = new StringBuilder();
        List<string> processed = new List<string>();
        for (int i = 0; i < paths.Length; i++)
        {
            if (!string.IsNullOrEmpty(paths[i]))
                processed.Add(paths[i]);
        }
        string current, next;
        for (int i = 0; i < processed.Count - 1; i++)
        {
            current = processed[i];
            next = processed[i + 1];
            sb.Append(current);
            if (!current[current.Length - 1].IsPathSeparater() && !next[0].IsPathSeparater())
                sb.Append('/');
        }
        sb.Append(processed[processed.Count - 1]);
        return sb.ToString().Replace('\\', '/');
    }

    public static string CombinePath_Windows(params string[] strs)
    {
        return CombinePath(strs).Replace('/', '\\');
    }

    public static bool IsPathSeparater(this char c)
    {
        return c == '\\' || c == '/';
    }

    public static FileInfo FindFile(string directoryPath, string fileName)
    {
        if (Directory.Exists(directoryPath))
        {
            DirectoryInfo d = new DirectoryInfo(directoryPath);
            FileInfo[] fs = d.GetFiles("*", SearchOption.AllDirectories);
            foreach (FileInfo info in fs)
            {
                if (info.Name == fileName)
                    return info;
            }
        }
        return null;
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
