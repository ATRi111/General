using System;

namespace Services.Save
{
    public static class SaveUtility
    {
        internal static void Write(string savePath, SaveDataGroup collection)
        {
            try
            {
                JsonTool.SaveAsJson(collection, savePath);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal static SaveDataGroup Read(string savePath)
        {
            try
            {
                return JsonTool.LoadFromJson<SaveDataGroup>(savePath) ?? new SaveDataGroup();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}