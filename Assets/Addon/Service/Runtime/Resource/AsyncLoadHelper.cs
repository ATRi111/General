using System;
using UnityEngine;

namespace Services
{
    internal class AsyncLoadHelper<T> where T : UnityEngine.Object
    {
        private readonly ResourceRequest request;
        private readonly Action<T> callBack;

        public AsyncLoadHelper(ResourceRequest request, Action<T> callBack)
        {
            this.request = request;
            this.callBack = callBack;
        }

        public void AfterLoadAsset(AsyncOperation _)
        {
            T asset = request.asset as T;
            if (asset == null)
                Debug.LogWarning("º”‘ÿ◊ ‘¥ ß∞‹");
            callBack?.Invoke(asset);
        }
    }
}