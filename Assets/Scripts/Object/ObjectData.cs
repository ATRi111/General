using UnityEngine;

namespace ObjectPool
{
    public class ObjectData
    {
        internal EObject EObject { get; private set; }
        /// <summary>
        /// 此物体在对象池中预生成的数量（对象池可以动态增大）
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

