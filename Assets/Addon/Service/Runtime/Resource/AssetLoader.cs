using System;
using UnityEngine;

namespace Services
{
    public class AssetLoader : Service
    {
        private readonly SceneControllerBase sceneController;

        internal AssetLoaderCore core;

        protected override void Awake()
        {
            base.Awake();
            core = new AssetLoaderCore();
        }

        protected internal override void Init()
        {
            base.Init();
            sceneController.AsyncLoadScene += OnLoadScene;
        }

        public T Load<T>(string path) where T : UnityEngine.Object
        {
            try
            {
                return core.Load<T>(path);
            }
            catch(Exception e)
            {
                Debug.LogError($"无法加载资源，资源路径为{path}");
                Debug.LogError(e);
                return null;
            }
        }

        public void LoadAsync<T>(string path, Action<T> callBack) where T : UnityEngine.Object
        {
            try
            {
                core.LoadAsync(path, callBack);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
                Debug.LogError($"无法加载资源，资源路径为{path}");
            }
        }

        public void UnLoadAsset<T>(T asset) where T : UnityEngine.Object
            => core.UnLoadAsset(asset);

        private void OnLoadScene(AsyncOperation _)
        {
            AutoUnload();
        }

        internal void AutoUnload()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}