using UnityEngine;
using UnityEditor;

public class Character : MonoBehaviour
{
    public int health;

    #region 保存或撤销对部分角色参数的修改
#if UNITY_EDITOR
    [Header("保存的部分角色参数")]
    [ContextMenuItem("保存角色参数（editormode和playmode均有效）", "Save")]
    [ContextMenuItem("读取之前保存的角色参数（editormode有效，playmode临时有效）", "Load")]
    [SerializeField]
    private CharacterData data;
    protected virtual void Save()
    {
        Debug.Log($"{gameObject.name}的参数已保存在{AssetDatabase.GetAssetPath(data)}");
    }
    protected virtual void Load()
    {
        Debug.Log($"{gameObject.name}的参数已保存在{AssetDatabase.GetAssetPath(data)}");
    }
#endif
    #endregion
}