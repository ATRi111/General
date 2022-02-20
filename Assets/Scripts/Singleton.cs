using UnityEngine;

/// <summary>
/// �̳�����༴��ʵ�ֵ���
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
            if (Instance == null) Debug.Log("��������ʧ��");
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
