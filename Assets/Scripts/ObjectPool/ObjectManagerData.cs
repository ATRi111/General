using UnityEngine;

namespace ObjectPool
{
    [CreateAssetMenu]
    public class ObjectManagerData : ScriptableObject
    {
        public ObjectPoolData[] datas;
    }
}