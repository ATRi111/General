using System;

namespace Services
{
    public class SaveManagerCore
    {
        internal WholeSaveData RuntimeData { get; set; }

        internal void Write(string savePath)
        {
            try
            {
                JsonTool.SaveAsJson(RuntimeData, savePath);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal void Read(string savePath)
        {
            try
            {
                RuntimeData = JsonTool.LoadFromJson<WholeSaveData>(savePath);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                RuntimeData ??= new WholeSaveData();
            }
        }
    }
}