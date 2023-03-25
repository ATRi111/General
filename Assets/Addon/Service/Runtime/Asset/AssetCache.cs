using System;
using System.Collections.Generic;

namespace Services.Asset
{
    internal class AssetCache
    {
        internal Dictionary<AssetIdentifier, AssetHandle> cache;

        internal AssetCache()
        {
            cache = new Dictionary<AssetIdentifier, AssetHandle>();
        }

        internal UnityEngine.Object Read(Type type, string address)
        {
            AssetIdentifier id = new AssetIdentifier(type, address);
            if (cache.ContainsKey(id))
            {
                cache[id].count++;
                return cache[id].asset;
            }
            return null;
        }

        internal void Add(Type type, string address, AssetHandle handle)
        {
            AssetIdentifier id = new AssetIdentifier(type, address);
            if (!cache.ContainsKey(id))
                cache.Add(id, handle);
            else
            {
                cache[id].count++;
                handle.Release();
            }
        }

        internal void Release(Type type, string address)
        {
            AssetIdentifier id = new AssetIdentifier(type, address);
            cache[id].count--;
            if (cache[id].count == 0)
            {
                cache[id].Release();
                cache.Remove(id);
            }
        }

        internal void Debug()
        {
            foreach (KeyValuePair<AssetIdentifier, AssetHandle> pair in cache)
            {
                Debugger.Log($"({pair.Key.address},{pair.Value.count})");
            }
        }
    }

    internal struct AssetIdentifier
    {
        internal Type type;
        internal string address;

        internal AssetIdentifier(Type type, string address)
        {
            this.type = type;
            this.address = address;
        }

        public override bool Equals(object obj)
        {
            if (obj is not AssetIdentifier identifier)
                return false;

            return type == identifier.type && address == identifier.address;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(type, address);
        }
    }

    internal abstract class AssetHandle
    {
        internal UnityEngine.Object asset;

        internal int count;

        internal AssetHandle(UnityEngine.Object asset)
        {
            this.asset = asset;
            count = 1;
        }

        internal abstract void Release();
    }

    internal class AssetCacheHelper
    {
        internal AssetCache cache;
        internal Type type;
        internal string address;

        internal AssetCacheHelper(AssetCache cache, Type type, string address)
        {
            this.cache = cache;
            this.type = type;
            this.address = address;
        }

        internal void WaitForAssetHandle(AssetHandle assetHandle)
        {
            cache.Add(type, address, assetHandle);
        }
    }
}