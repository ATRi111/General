using UnityEngine;
using UnityEditor;

public class Character : MonoBehaviour
{
    #region ��������Բ��ֽ�ɫ�������޸�
    [Header("����Ĳ��ֽ�ɫ����")]
    [ContextMenuItem("�����ɫ������editormode��playmode����Ч��", "Save")]
    [ContextMenuItem("��ȡ֮ǰ����Ľ�ɫ������editormode��Ч��playmode��ʱ��Ч��", "Load")]
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