using System;
using UnityEngine;

namespace Services.Asset
{
    internal class AsyncLoadHelper_Resource<T> where T : UnityEngine.Object
    {
        private readonly ResourceRequest request;
        private readonly Action<T> callBack;
        private readonly Action<AssetHandle> cacheCallBack;

        public AsyncLoadHelper_Resource(ResourceRequest request, Action<T> callBack, Action<AssetHandle> cacheCallBack)
        {
            this.request = request;
            this.callBack = callBack;
            this.cacheCallBack = cacheCallBack;
            request.completed += AfterLoadAsset;
        }

        public void AfterLoadAsset(AsyncOperation _)
        {
            if (request.asset != null)
            {
                T asset = request.asset as T;
                if (asset != null)
                {
                    cacheCallBack?.Invoke(new AssetHandle_Resource(asset));
                    callBack?.Invoke(asset);
                }
                else
                    throw new InvalidCastException();
            }

        }
    }
}