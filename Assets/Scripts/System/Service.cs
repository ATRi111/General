using UnityEngine;

/// <summary>
/// 服务，从服务定位器获取
/// </summary>
public abstract class Service : MonoBehaviour
{
    /// <summary>
    /// 如果要在Awake中获取其他服务，注意脚本执行顺序
    /// </summary>
    protected virtual void Awake()
    {
        ServiceLocator.Register(this);
    }
}
