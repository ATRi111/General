using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoader_Resoureces : AssetLoader
{
    public override T LoadResource<T>(string path)
    {
        return Resources.Load<T>(path);
    }

    public override void LoadResourceAsync<T>(string path, Action<T> callBack)
    {
        ResourceRequest request = Resources.LoadAsync<T>(path);
        StartCoroutine(WaitForLoad(request, callBack));
    }

    public override void UnLoadResource<T>(T t)
    {
        Resources.UnloadAsset(t);
    }

    private IEnumerator WaitForLoad<T>(ResourceRequest request, Action<T> callBack) where T : UnityEngine.Object
    {
        for (;!request.isDone ; )
        {
            yield return null;
        }
        callBack?.Invoke(request.asset as T);
    }
}
