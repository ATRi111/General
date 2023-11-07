using Newtonsoft.Json;
using System;
using System.IO;

namespace Services
{
    public static class JsonTool
    {
        public static JsonSerializerSettings DefaultSettings;

        static JsonTool()
        {
            DefaultSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
            };
        }

        /// <summary>
        /// 保存为json
        /// </summary>
        /// <param name="path">路径，要加拓展名</param>
        public static void SaveAsJson<T>(T t, string path)
        {
            try
            {
                FileInfo info = FileTool.GetFileInfo(path, true);
                using StreamWriter writer = info.CreateText();
                string str = JsonConvert.SerializeObject(t, Formatting.Indented, DefaultSettings);
                writer.WriteLine(str);
            }
            catch (Exception e)
            {
                Debugger.LogError(e.ToString(), EMessageType.System);
            }
        }

        /// <summary>
        /// 读取json
        /// </summary>
        /// <param name="path">路径，要加拓展名</param>
        public static T LoadFromJson<T>(string path) where T : class
        {
            FileTool.GetFileInfo(path);
            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(path), DefaultSettings);
            }
            catch (Exception e)
            {
                Debugger.LogWarning(e.ToString(), EMessageType.System);
                return null;
            }
        }
    }
}