using System.Collections.Generic;
using UnityEngine;

namespace Services.ObjectPools
{
    internal class ObjectManagerCore
    {
        internal Dictionary<string, ObjectPool> objectPools;
        private readonly MonoBehaviour mono;
        private readonly IObjectLocator locator;

        public ObjectManagerCore(MonoBehaviour mono, ObjectLocatorBase locator)
        {
            this.mono = mono;
            this.locator = locator;
            objectPools = new Dictionary<string, ObjectPool>();
        }

        internal IMyObject Activate(string identifier, Vector3 position, Vector3 eulerAngles, Transform parent)
        {
            if (!objectPools.ContainsKey(identifier))
                CreatePool(identifier);
            IMyObject obj = objectPools[identifier].Activate(position, eulerAngles, parent);
            return obj;
        }

        internal void CreatePool(string identifier)
        {
            GameObject obj_pool = new GameObject($"Pool:{identifier}");
            obj_pool.transform.parent = mono.transform;
            ObjectPool pool = obj_pool.AddComponent<ObjectPool>();
            GameObject prefab = locator.Locate(identifier);
            if (prefab != null)
            {
                pool.Initialize(prefab);
                objectPools.Add(identifier, pool);
            }
        }

        internal void PreCreate(string identifier, int count)
        {
            if (!objectPools.ContainsKey(identifier))
                CreatePool(identifier);
            objectPools[identifier].Create(count);
        }
    }
}

