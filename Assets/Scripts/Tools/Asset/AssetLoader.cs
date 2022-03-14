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
    /// ����
    /// </summary>
    private static readonly Dictionary<string, UnityEngine.Object> buffer = new Dictionary<string, UnityEngine.Object>();

    /// <summary>
    /// ������Դ
    /// </summary>
    /// <param name="path">��Դ·��</param>
    /// <param name="callBack">�ص����������غõ���Դ��Ϊ�������룩</param>
    /// <param name="async">�Ƿ��첽</param>
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
            Debug.LogWarning($"�޷�������Դ��·��Ϊ{path}");
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
