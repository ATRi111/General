using System;
using UnityEngine;

namespace Services.Asset
{
    internal class AssetLoaderCore_Resource : AssetLoaderCore
    {
        protected override T Load_Miss<T>(string address, Action<AssetHandle> cacheCallBack)
        {
            T ret = Resources.Load<T>(address);
            if (ret != null)
                cacheCallBack?.Invoke(new AssetHandle_Resource(ret));
            return ret;
        }

        protected override void LoadAsync_Miss<T>(string address, Action<T> callBack, Action<AssetHandle> cacheCallBack)
        {
            ResourceRequest request = Resources.LoadAsync<T>(address);
            new AsyncLoadHelper_Resource<T>(request, callBack, cacheCallBack);
        }
    }
}