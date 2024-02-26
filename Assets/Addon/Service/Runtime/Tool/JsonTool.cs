using Newtonsoft.Json;
using System;
using System.IO;

namespace Services
{
    public static class JsonTool
    {
        public readonly static JsonSerializerSettings DefaultSettings;
        /// <summary>
        /// 涉及多态反序列化时,使用此设置
        /// </summary>
        public readonly static JsonSerializerSettings PolyMorphicSettings;

        static JsonTool()
        {
            DefaultSettings = new JsonSerializerSettings();
            PolyMorphicSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            };
        }

        /// <summary>
        /// 保存为json
        /// </summary>
        /// <param name="path">路径，要加拓展名</param>
        public static void SaveAsJson<T>(T t, string path)
            => SaveAsJson(t, path, DefaultSettings);

        public static void SaveAsJson<T>(T t, string path, JsonSerializerSettings settings)
        {
            try
            {
                FileInfo info = FileTool.GetFileInfo(path, true);
                using StreamWriter writer = info.CreateText();
                string str = JsonConvert.SerializeObject(t, Formatting.Indented, settings);
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
            => LoadFromJson<T>(path, DefaultSettings);

        public static T LoadFromJson<T>(string path, JsonSerializerSettings settings) where T : class
        {
            FileTool.GetFileInfo(path);
            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(path), settings);
            }
            catch (Exception e)
            {
                Debugger.LogWarning(e.ToString(), EMessageType.System);
                return null;
            }
        }
    }
}