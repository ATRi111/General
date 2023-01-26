﻿using UnityEngine;

namespace Services.Asset
{
    internal abstract class AssetLoaderCore
    {
        internal AssetCache cache;

        public AssetLoaderCore()
        {
            cache = new AssetCache();
        }

        internal T Load<T>(string address) where T : Object
        {
            T ret = cache.Read(typeof(T), address) as T;
            if (ret != null)
                return ret;

            AssetCacheHelper helper = new AssetCacheHelper(cache, typeof(T), address);
            ret = Load_Miss<T>(address, helper.WaitForAssetHandle);
            return ret;
        }
        //缓存未命中时调用
        protected abstract T Load_Miss<T>(string address, System.Action<AssetHandle> cacheCallBack) where T : Object;

        internal void LoadAsync<T>(string address, System.Action<T> callBack) where T : Object
        {
            T ret = cache.Read(typeof(T), address) as T;
            if (ret != null)
            {
                callBack?.Invoke(ret);
                return;
            }

            AssetCacheHelper helper = new AssetCacheHelper(cache, typeof(T), address);
            LoadAsync_Miss(address, callBack, helper.WaitForAssetHandle);
        }
        //缓存未命中时调用
        protected abstract void LoadAsync_Miss<T>(string address, System.Action<T> callBack, System.Action<AssetHandle> cacheCallBack) where T : Object;

        internal void Release<T>(string address) where T : Object
        {
            cache.Release(typeof(T), address);
        }
    }
}