using UnityEngine;

//继承此类的脚本应该挂载ServiceLocator所在的游戏物体或其子物体上，否则可能要调用DontDestroyOnLoad(gameObject)
public abstract class Service : MonoBehaviour
{
    protected EService eService;

    protected void Awake()
    {
        BeforeRegister();
        if (eService == EService.Default)
            Debug.LogWarning("没有为服务指定枚举类型");
        ServiceLocator.Instance.Register(eService, this);
    }
    /// <summary>
    /// Service自动将自身登记到ServiceLocator前的行为，为Service分配枚举是必要的
    /// </summary>
    protected abstract void BeforeRegister();
}
