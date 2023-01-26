using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Services.Asset
{
    internal class AssetLoaderCore_Addressable : AssetLoaderCore
    {
        protected override T Load_Miss<T>(string address, Action<AssetHandle> cacheCallBack)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            T ret = handle.WaitForCompletion();
            if (ret != null)
                cacheCallBack?.Invoke(new AssetHandle_Addressable(ret, handle));
            return ret;
        }

        protected override void LoadAsync_Miss<T>(string address, Action<T> callBack, Action<AssetHandle> cacheCallBack)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            new AsyncLoadHelper_Addressable<T>(handle, callBack, cacheCallBack);
        }
    }
}