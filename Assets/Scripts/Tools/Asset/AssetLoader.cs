using System;
using UnityEngine;

//路径不能包含扩展名
public abstract class AssetLoader :Service
{
    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">路径，不能包含拓展名</param>
    public abstract T LoadAsset<T>(string path) where T : UnityEngine.Object;

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">路径，不能包含拓展名</param>
    /// <param name="callBack">回调函数，参数相当于返回值</param>
    public abstract void LoadAssetAsync<T>(string path,Action<T> callBack) where T : UnityEngine.Object;

    public abstract void UnLoadAsset<T>(T t) where T : UnityEngine.Object;
}
