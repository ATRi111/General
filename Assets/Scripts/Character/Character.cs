using UnityEngine;
using UnityEditor;

public class Character : MonoBehaviour
{
    #region 保存或撤销对角色参数的修改
    [ContextMenuItem("保存角色参数", "Save")]
    [ContextMenuItem("读取之前保存的角色参数", "Load")]
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