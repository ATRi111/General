using UnityEngine;
using UnityEditor;

public class Character : MonoBehaviour
{
    public int health;

    #region ��������Բ��ֽ�ɫ�������޸�
#if UNITY_EDITOR
    [Header("����Ĳ��ֽ�ɫ����")]
    [ContextMenuItem("�����ɫ������editormode��playmode����Ч��", "Save")]
    [ContextMenuItem("��ȡ֮ǰ����Ľ�ɫ������editormode��Ч��playmode��ʱ��Ч��", "Load")]
    [SerializeField]
    private CharacterData data;
    protected virtual void Save()
    {
        Debug.Log($"{gameObject.name}�Ĳ����ѱ�����{AssetDatabase.GetAssetPath(data)}");
    }
    protected virtual void Load()
    {
        Debug.Log($"{gameObject.name}�Ĳ����ѱ�����{AssetDatabase.GetAssetPath(data)}");
    }
#endif
    #endregion
}