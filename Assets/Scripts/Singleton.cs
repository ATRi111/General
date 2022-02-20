using UnityEngine;

/// <summary>
/// 继承这个类即可实现单例
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
            if (Instance == null) Debug.Log("创建单例失败");
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
