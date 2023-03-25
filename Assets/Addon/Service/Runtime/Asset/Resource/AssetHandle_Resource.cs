using UnityEngine;

namespace Services.Asset
{
    internal class AssetHandle_Resource : AssetHandle
    {
        public AssetHandle_Resource(Object asset) : base(asset)
        {
        }

        internal override void Release()
        {
            Resources.UnloadAsset(asset);
        }
    }
}