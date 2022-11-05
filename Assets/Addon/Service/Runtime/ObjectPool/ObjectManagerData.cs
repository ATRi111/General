using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    internal class ObjectManagerData : ScriptableObject
    {
        [SerializeField]
        internal EObjectPrefabPair[] datas;
        [SerializeField]
        internal Dictionary<EObject, GameObject> objectDict;

        public void Initialize()
        {
            objectDict = new Dictionary<EObject, GameObject>();
            foreach (EObjectPrefabPair pair in datas)
            {
                objectDict.Add(pair.eObject, pair.prefab);
            }
        }
    }
}