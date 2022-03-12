using UnityEngine;


//为了不在场景切换时被销毁，继承此类的脚本通常挂载ServiceLocator所在的游戏物体或其子物体上，
/// <summary>
/// 服务，从服务定位器获取
/// </summary>
public abstract class Service : MonoBehaviour
{
    protected virtual void Awake()
    {
        ServiceLocator.Instance.Register(this);
    }

}
