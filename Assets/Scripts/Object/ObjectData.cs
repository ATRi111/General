using UnityEngine;

namespace ObjectPool
{
    public class ObjectData
    {
        internal EObject EObject { get; private set; }
        /// <summary>
        /// �������ڶ��������Ҫ��������0��ʾ��ʹ�ö����
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

