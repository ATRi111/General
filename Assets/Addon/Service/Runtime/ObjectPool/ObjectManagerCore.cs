using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    internal class ObjectManagerCore
    {
        internal Dictionary<EObject, ObjectPool> objectPools;
        private readonly MonoBehaviour mono;
        private readonly ObjectManagerData data;

        public ObjectManagerCore(MonoBehaviour mono, ObjectManagerData data)
        {
            this.mono = mono;
            this.data = data;
            data.Initialize();
            objectPools = new Dictionary<EObject, ObjectPool>();
        }

        internal IMyObject Activate(EObject eObject, Vector3 position, Vector3 eulerAngles, Transform parent)
        {
            if (!objectPools.ContainsKey(eObject))
                CreatePool(eObject);
            IMyObject obj = objectPools[eObject].Activate(position, eulerAngles, parent);
            return obj;
        }

        internal ObjectPool CreatePool(EObject eObject)
        {
            GameObject obj_pool = new GameObject($"Pool:{eObject}");
            obj_pool.transform.parent = mono.transform;
            ObjectPool pool = obj_pool.AddComponent<ObjectPool>();
            pool.Initialize(data.objectDict[eObject]);
            objectPools.Add(eObject, pool);
            return pool;
        }

        internal void PreCreate(EObject eObject, int count)
        {
            if (!objectPools.ContainsKey(eObject))
                CreatePool(eObject);
            objectPools[eObject].Create(count);
        }
    }
}

