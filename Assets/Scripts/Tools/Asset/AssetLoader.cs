using System;
using UnityEngine;


//路径不能包含扩展名
public abstract class AssetLoader :MonoBehaviour
{
    public abstract T LoadResource<T>(string path) where T : UnityEngine.Object;

    public abstract void LoadResourceAsync<T>(string path,Action<T> callBack) where T : UnityEngine.Object;

    public abstract void UnLoadResource<T>(T t) where T : UnityEngine.Object;
    public void AutoUnLoad()
    {
        Resources.UnloadUnusedAssets();
    }
}
