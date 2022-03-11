using UnityEngine;

//�̳д���Ľű�Ӧ�ù���ServiceLocator���ڵ���Ϸ��������������ϣ��������Ҫ����DontDestroyOnLoad(gameObject)
public abstract class Service : MonoBehaviour
{
    protected EService eService;

    protected void Awake()
    {
        BeforeRegister();
        if (eService == EService.Default)
            Debug.LogWarning("û��Ϊ����ָ��ö������");
        ServiceLocator.Instance.Register(eService, this);
    }
    /// <summary>
    /// �ڴ˷�����ΪService����ö�ٳ�������ִ��������ʼ����Ϊ
    /// </summary>
    protected abstract void BeforeRegister();
}
