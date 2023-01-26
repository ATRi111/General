using System;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Services.Asset
{
    internal class AsyncLoadHelper_Addressable<T> where T : UnityEngine.Object
    {
        private readonly Action<T> callBack;
        private readonly Action<AssetHandle> cacheCallback;
        internal AsyncLoadHelper_Addressable(AsyncOperationHandle handle, Action<T> callBack, Action<AssetHandle> cacheCallback)
        {
            this.callBack = callBack;
            this.cacheCallback = cacheCallback;
            handle.Completed += OnComplete;
        }

        private void OnComplete(AsyncOperationHandle handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                T ret = handle.Result as T;
                if (ret != null)
                {
                    cacheCallback?.Invoke(new AssetHandle_Addressable(ret, handle));
                    callBack?.Invoke(ret);
                }
                else
                    throw new InvalidCastException();
            }
        }
    }
}