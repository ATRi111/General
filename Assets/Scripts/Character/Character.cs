using UnityEngine;
using UnityEditor;

public class Character : MonoBehaviour
{
    #region ��������Խ�ɫ�������޸�
    [ContextMenuItem("�����ɫ����", "Save")]
    [ContextMenuItem("��ȡ֮ǰ����Ľ�ɫ����", "Load")]
    [SerializeField]
    private CharacterData data;
    private void Save()
    {
        Debug.Log($"��ɫ�����ѱ�����{AssetDatabase.GetAssetPath(data)}");
    }
    private void Load()
    {
        Debug.Log($"�Ѵ�{AssetDatabase.GetAssetPath(data)}��ȡ��ɫ����");
    }
    #endregion
}