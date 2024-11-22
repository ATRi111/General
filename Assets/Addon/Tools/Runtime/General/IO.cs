using System.IO;

namespace MyTool
{
    public static partial class GeneralTool
    {
        public static void DeleteAllFiles(string path)
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
                string[] directories = Directory.GetDirectories(path);
                foreach (string directory in directories)
                {
                    Directory.Delete(directory, true);
                }
            }
        }
    }
}