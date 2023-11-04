using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Services
{
    public static class JsonTool
    {
        /// <summary>
        /// 保存为json
        /// </summary>
        /// <param name="path">路径，要加拓展名</param>
        public static void SaveAsJson<T>(T t, string path, params JsonConverter[] converters)
        {
            try
            {
                FileInfo info = FileTool.GetFileInfo(path, true);
                using StreamWriter writer = info.CreateText();
                string str = JsonConvert.SerializeObject(t, Formatting.Indented, converters);
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
        public static T LoadFromJson<T>(string path, params JsonConverter[] converters) where T : class
        {
            FileTool.GetFileInfo(path);
            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(path), converters);
            }
            catch (Exception e)
            {
                Debugger.LogWarning(e.ToString(), EMessageType.System);
                return null;
            }
        }
    }

    public class PolyConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                JObject jObject = JObject.Load(reader);
                Type type = jObject["_exactType"].ToObject<Type>();
                object ret = jObject.ToObject(type);
                serializer.Populate(reader, ret);
                return ret;
            }
            catch(Exception e)
            {
                Debugger.LogException(e, EMessageType.System);
                return null;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            try
            {
                //Debug.Log(value.ToString());
                //JToken token = JToken.FromObject(value);
                //JObject jObject = (JObject)token;
                //jObject.Add("_exactType", JToken.FromObject(value.GetType()));
                //Debug.Log(jObject.ToString());
                //jObject.WriteTo(writer);
            }
            catch (Exception e)
            {
                Debugger.LogException(e, EMessageType.System);
            }
        }
    }
}