using UnityEngine;

namespace ObjectPool
{
    public enum EObject
    {

    }

    /// <summary>
    /// ������������������
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

