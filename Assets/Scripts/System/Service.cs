using UnityEngine;

public abstract class Service : MonoBehaviour
{
    protected EService eService;
    protected virtual void Awake()
    {
        if (eService == EService.Default)
            Debug.LogWarning("û��Ϊ����ָ��ö������");
        ServiceLocator.Instance.AddService(eService, this);
    }
}
