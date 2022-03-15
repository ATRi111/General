using System;
using UnityEngine;

//路径不能包含扩展名
public class AssetLoader_Resoureces : AssetLoader
{
    private class Coupling<T> where T : UnityEngine.Object
    {
        private readonly ResourceRequest request;
        private readonly Action<T> callBack;

        public Coupling(ResourceRequest _request, Action<T> _callBack)
        {
            request = _request;
            callBack = _callBack;
        }

        public void OnCompleteOperation(AsyncOperation asyncOperation)
        {
            T asset = request.asset as T;
            if (asset == null)
                Debug.LogWarning("加载资源失败");
            callBack?.Invoke(asset);
        }
    }
   
    protected override void Load<T>(string path, Action<T> callBack)
    {
        T asset = Resources.Load<T>(path);
        callBack?.Invoke(asset);
    }

    protected override void LoadAsync<T>(string path, Action<T> callBack)
    {
        ResourceRequest request = Resources.LoadAsync<T>(path);
        Coupling<T> intermediary = new Coupling<T>(request, callBack);
        request.completed += intermediary.OnCompleteOperation;
    }

    public override void UnLoadAsset<T>(T t)
    {
        Resources.UnloadAsset(t);
    }
}
