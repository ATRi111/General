using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Services.Asset
{
    internal class AssetHandle_Addressable : AssetHandle
    {
        private AsyncOperationHandle handle;

        public AssetHandle_Addressable(Object asset, AsyncOperationHandle handle) : base(asset)
        {
            this.handle = handle;
        }

        internal override void Release()
        {
            Addressables.Release(handle);
        }
    }
}