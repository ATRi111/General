using UnityEngine;

/// <summary>
/// �̳�����༴��ʵ�ֵ�����ע�ⳡ���л�ʱ���������ڵ���Ϸ������ܻᱻ����
/// </summary>
/// <typeparam name="T">�̳�Singleton���༴ΪT</typeparam>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else
            Destroy(gameObject);
    }
}
