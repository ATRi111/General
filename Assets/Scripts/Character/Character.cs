using UnityEngine;

public class Character : MonoBehaviour
{
    #region 保存或撤销对角色参数的修改
    [ContextMenuItem("Save", "Save")]
    [ContextMenuItem("Load", "Load")]
    [SerializeField]
    private CharacterData data;
    private void Save()
    {

    }
    private void Load()
    {

    }
    #endregion
}