using UnityEngine;

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
    /// Service�Զ�������Ǽǵ�ServiceLocatorǰ����Ϊ��ΪService����ö���Ǳ�Ҫ��
    /// </summary>
    protected abstract void BeforeRegister();
}
