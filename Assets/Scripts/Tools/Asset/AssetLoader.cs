using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-200)]
public abstract class AssetLoader : Service
{
    private class BufferUpdate<T> where T : UnityEngine.Object
    {
        public string path;
        public T asset;

        public BufferUpdate(string _path)
        {
            path = _path;
        }

        public void AfterLoadAsset(T _asset)
        {
            asset = _asset;
        }
    }

    /// <summary>
    /// 缓存
    /// </summary>
    private static readonly Dictionary<string, UnityEngine.Object> buffer = new Dictionary<string, UnityEngine.Object>();

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="path">资源路径</param>
    /// <param name="callBack">回调函数（加载好的资源作为参数传入）</param>
    /// <param name="async">是否异步</param>
    public void LoadAsset<T>(string path, Action<T> callBack, bool async = true) where T : UnityEngine.Object
    {
        try
        {
            if (buffer.ContainsKey(path))
                callBack?.Invoke(buffer[path] as T);
            else
            {
                BufferUpdate<T> request = new BufferUpdate<T>(path);
                callBack += request.AfterLoadAsset;
                if (async)
                    LoadAsync<T>(path, callBack);
                else
                    Load<T>(path, callBack);
            }
        }
        catch
        {
            Debug.LogWarning($"无法加载资源，路径为{path}");
        }
    }

    protected abstract void Load<T>(string path, Action<T> callBack) where T : UnityEngine.Object;

    protected abstract void LoadAsync<T>(string path, Action<T> callBack) where T : UnityEngine.Object;

    public abstract void UnLoadAsset<T>(T asset) where T : UnityEngine.Object;

    private void UpdateBuffer<T>(BufferUpdate<T> request) where T : UnityEngine.Object
    {
        buffer.Add(request.path, request.asset);
    }

}
