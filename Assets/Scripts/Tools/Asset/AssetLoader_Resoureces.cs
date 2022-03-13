using System;
using System.Collections;
using UnityEngine;

public class AssetLoader_Resoureces : AssetLoader
{
    public override T LoadAsset<T>(string path)
    {
        return Resources.Load<T>(path);
    }

    public override void LoadAssetAsync<T>(string path, Action<T> callBack)
    {
        ResourceRequest request = Resources.LoadAsync<T>(path);
        StartCoroutine(WaitForLoad(request, callBack,path));
    }

    public override void UnLoadAsset<T>(T t)
    {
        Resources.UnloadAsset(t);
    }

    private IEnumerator WaitForLoad<T>(ResourceRequest request, Action<T> callBack,string path) where T : UnityEngine.Object
    {
        yield return request;
        try
        {
            T asset = request.asset as T;
            callBack?.Invoke(asset);
        }
        catch
        {
            Debug.LogWarning($"无法加载资源，路径为{path}");
        }
    }
}
