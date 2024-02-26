using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.Save
{
    public static class SaveUtility
    {
        /// <summary>
        /// 默认的用于确定标识符的方法，即对象名+场景名
        /// </summary>
        public static string DefineIdentifier_SceneAndName(Object obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(SceneManager.GetActiveScene().name);
            sb.Append("-");
            sb.Append(obj.name);
            return sb.ToString();
        }

        /// <summary>
        /// 生成存档路径
        /// </summary>
        /// <param name="fileName">文件名，必须包含后缀</param>
        public static string GenerateSavePath(string fileName)
        {
            return FileTool.CombinePath(Application.persistentDataPath, fileName);
        }

        internal static void Write(string savePath, SaveDataGroup group)
        {
            JsonTool.SaveAsJson(group, savePath, JsonTool.PolyMorphicSettings);
        }

        internal static SaveDataGroup Read(string savePath)
        {
            SaveDataGroup ret = JsonTool.LoadFromJson<SaveDataGroup>(savePath, JsonTool.PolyMorphicSettings);
            if (ret == null)
            {
                Debugger.LogWarning("无法读取存档，创建新存档", EMessageType.Save);
                ret = new SaveDataGroup();
            }
            return ret;
        }
    }
}