using UnityEngine;
using UnityEditor;

public class Character : MonoBehaviour
{
    #region 保存或撤销对部分角色参数的修改
    [Header("保存的部分角色参数")]
    [ContextMenuItem("保存角色参数（editormode和playmode均有效）", "Save")]
    [ContextMenuItem("读取之前保存的角色参数（editormode有效，playmode临时有效）", "Load")]
    [SerializeField]
    private CharacterData data;
    private void Save()
    {
        Debug.Log($"角色参数已保存在{AssetDatabase.GetAssetPath(data)}");
    }
    private void Load()
    {
        Debug.Log($"已从{AssetDatabase.GetAssetPath(data)}读取角色参数");
    }
    #endregion
}