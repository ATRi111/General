using UnityEngine;

public class SaveManager : Service
{
    [SerializeField]
    private SaveData _Data;
    /// <summary>
    /// ��������Ϸ������
    /// </summary>
    public SaveData Data => _Data;
    [SerializeField]
    private string savePath;

    public void Save()
    {
        JsonTool.SaveAsJson(Data, savePath);
    }

    public void Load()
    {
        _Data = JsonTool.LoadFromJson<SaveData>(savePath);
    }
}

[System.Serializable]
public class SaveData
{

}