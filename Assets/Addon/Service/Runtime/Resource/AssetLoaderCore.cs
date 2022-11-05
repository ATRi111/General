using System;
using UnityEngine;

namespace Services
{
    internal class AssetLoaderCore
    {
        internal T Load<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        internal void LoadAsync<T>(string path, Action<T> callBack) where T : UnityEngine.Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(path);
            AsyncLoadHelper<T> coupling = new AsyncLoadHelper<T>(request, callBack);
            request.completed += coupling.AfterLoadAsset;
        }

        internal void UnLoadAsset<T>(T asset) where T : UnityEngine.Object
        {
            Resources.UnloadAsset(asset);
        }
    }
}