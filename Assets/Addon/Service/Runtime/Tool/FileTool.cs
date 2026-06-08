using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Services
{
    public static class FileTool
    {
        public static Encoding DefaultEncoding = Encoding.UTF8;

        public static string CombinePath(params string[] paths)
        {
            string result = Path.Combine(paths);
            return result.Replace('\\', '/');
        }

        public static string CombinePath_Windows(params string[] strs)
        {
            return CombinePath(strs).Replace('/', '\\');
        }

        public static bool IsPathSeparator(this char c)
        {
            return c == '\\' || c == '/';
        }

        public static FileInfo FindFile(string directoryPath, string fileName)
        {
            if (Directory.Exists(directoryPath))
            {
                DirectoryInfo d = new(directoryPath);
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
        /// 获取文件信息
        /// </summary>
        /// <param name="path">路径，要包含拓展名</param>
        /// <param name="create">文件不存在时，是否新创建文件</param>
        public static FileInfo GetFileInfo(string path, bool create = false)
        {
            FileInfo fileInfo = new(path);
            if (!fileInfo.Exists)
            {
                if (create)
                {
                    Directory.CreateDirectory(fileInfo.DirectoryName);
                    fileInfo.Create().Dispose();
                }
                else
                    Debugger.LogWarning($"{path}文件不存在", EMessageType.System);
            }
            return fileInfo;
        }
    }
}