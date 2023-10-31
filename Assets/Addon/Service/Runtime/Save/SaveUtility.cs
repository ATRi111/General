namespace Services.Save
{
    public static class SaveUtility
    {
        /// <summary>
        /// 生成存档路径
        /// </summary>
        /// <param name="fileName">文件名，必须包含后缀</param>
        public static string GenerateSavePath(string fileName)
        {
            return FileTool.CombinePath(UnityEngine.Application.persistentDataPath, fileName);
        }

        internal static void Write(string savePath, SaveDataGroup group)
        {
            Debugger.Log(group.ToString());
            JsonTool.SaveAsJson(group, savePath);
        }

        internal static SaveDataGroup Read(string savePath)
        {
            SaveDataGroup ret = JsonTool.LoadFromJson<SaveDataGroup>(savePath);
            if(ret == null)
            {
                Debugger.LogWarning("无法读取存档，创建新存档", EMessageType.Save);
                ret = new SaveDataGroup();
            }
            return ret;
        }
    }
}