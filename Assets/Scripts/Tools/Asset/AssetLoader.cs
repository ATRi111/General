using System;
using UnityEngine;

//路径不能包含扩展名
public abstract class AssetLoader : Service
{
    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="path">资源路径</param>
    /// <param name="async">是否异步</param>
    /// <param name="callBack">回调函数（加载好的资源作为参数传入）</param>
    public void LoadAsset<T>(string path, Action<T> callBack, bool async = true) where T : UnityEngine.Object
    {
        try
        {
            if (async)
                LoadAsync<T>(path, callBack);
            else
                Load<T>(path, callBack);
        }
        catch
        {
            Debug.LogWarning($"无法加载资源，路径为{path}");
        }
    }

    protected abstract void Load<T>(string path, Action<T> callBack) where T : UnityEngine.Object;

    protected abstract void LoadAsync<T>(string path, Action<T> callBack) where T : UnityEngine.Object;

    public abstract void UnLoadAsset<T>(T t) where T : UnityEngine.Object;
}
