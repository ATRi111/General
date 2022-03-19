using UnityEngine;

/// <summary>
/// 继承这个类即可实现单例，注意场景切换时，单例所在的游戏物体可能会被销毁
/// </summary>
/// <typeparam name="T">继承Singleton的类即为T</typeparam>
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
