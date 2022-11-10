using System;
using UnityEngine;

namespace Services
{
    [Serializable]
    public class SaveManagerCore
    {
        [SerializeField]
        private WholeSaveData runtimeData;
        internal WholeSaveData RuntimeData
        {
            get
            {
                runtimeData ??= new WholeSaveData();
                return runtimeData;
            }
            set => runtimeData = value;
        }

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