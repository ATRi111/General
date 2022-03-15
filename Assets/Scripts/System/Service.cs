using UnityEngine;

/// <summary>
/// 服务，从服务定位器获取
/// </summary>
public abstract class Service : MonoBehaviour
{
    /// <summary>
    /// 再脚本顺序不受控制的情况下，不要在Awake中获取其他服务
    /// </summary>
    protected virtual void Awake()
    {
        ServiceLocator.Register(this);
    }
}
