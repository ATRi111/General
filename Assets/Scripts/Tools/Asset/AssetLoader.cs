using System;
using UnityEngine;

//·�����ܰ�����չ��
public abstract class AssetLoader : Service
{
    /// <summary>
    /// ������Դ
    /// </summary>
    /// <param name="path">��Դ·��</param>
    /// <param name="async">�Ƿ��첽</param>
    /// <param name="callBack">�ص����������غõ���Դ��Ϊ�������룩</param>
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
            Debug.LogWarning($"�޷�������Դ��·��Ϊ{path}");
        }
    }

    protected abstract void Load<T>(string path, Action<T> callBack) where T : UnityEngine.Object;

    protected abstract void LoadAsync<T>(string path, Action<T> callBack) where T : UnityEngine.Object;

    public abstract void UnLoadAsset<T>(T t) where T : UnityEngine.Object;
}
