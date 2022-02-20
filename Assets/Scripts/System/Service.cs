using UnityEngine;

public abstract class Service : MonoBehaviour
{
    protected EService eService;
    protected virtual void Awake()
    {
        if (eService == EService.Default)
            Debug.LogWarning("没有为服务指定枚举类型");
        ServiceLocator.Instance.AddService(eService, this);
    }
}
