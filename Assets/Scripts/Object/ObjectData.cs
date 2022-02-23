using UnityEngine;

namespace ObjectPool
{
    public class ObjectData
    {
        internal EObject EObject { get; private set; }
        /// <summary>
        /// 此物体在对象池中需要的数量，0表示不使用对象池
        /// </summary>
        internal int NumInPool { get; private set; }
        internal GameObject Prefab { get; set; }
        internal ObjectData(EObject eObject, int num, GameObject prefab = null)
        {
            EObject = eObject;
            NumInPool = num;
            Prefab = prefab;
        }
    }
}

