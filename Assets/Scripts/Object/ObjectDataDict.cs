using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public enum EObject
    {

    }

    [CreateAssetMenu]
    public class ObjectDataDict : ScriptableObject, IInitialize
    {
        private readonly Dictionary<EObject, ObjectData> objectDict = new Dictionary<EObject, ObjectData>();

        [Header("顺序必须与EObject一致")]
        [SerializeField]
        private GameObject[] prefabs;
        [Header("顺序必须与EObject一致")]
        [SerializeField]
        private int[] nums;

        internal int NumOfObjects { get; private set; }   //由ObjectManager管理的游戏物体总数

        public void Initialize()
        {
            ObjectData data;
            NumOfObjects = nums.Length;
            for (int i = 0; i < NumOfObjects; i++)
            {
                data = new ObjectData((EObject)i, nums[i], prefabs[i]);
                objectDict.Add((EObject)i, data);
            }
        }

        internal ObjectData GetObject(EObject eObject)
        {
            return objectDict[eObject];
        }
    }
}