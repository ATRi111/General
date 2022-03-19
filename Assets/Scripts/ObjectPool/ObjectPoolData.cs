using UnityEngine;

namespace ObjectPool
{
    public enum EObject
    {

    }

    /// <summary>
    /// 创建对象池所需的数据
    /// </summary>
    [System.Serializable]
    public class ObjectPoolData
    {
        [SerializeField]
        internal EObject eObject;
        [SerializeField]
        internal int size;
        [SerializeField]
        internal GameObject prefab;
    }
}

