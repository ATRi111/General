using System;

namespace Services.Asset
{
    public interface IAssetLoader : IService
    {
        /// <summary>
        /// 同步加载资源
        /// </summary>
        T Load<T>(string address) where T : UnityEngine.Object;
        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="callBack">接受结果的回调函数</param>
        void LoadAsync<T>(string address, Action<T> callBack) where T : UnityEngine.Object;
        /// <summary>
        /// 释放资源（减少一次引用）
        /// </summary>
        void Release<T>(string address) where T : UnityEngine.Object;
    }
}