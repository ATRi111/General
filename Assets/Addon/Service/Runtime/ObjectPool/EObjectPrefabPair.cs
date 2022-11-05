using UnityEngine;

namespace Services
{
    [System.Serializable]
    public class EObjectPrefabPair
    {
        [SerializeField]
        internal EObject eObject;
        [SerializeField]
        internal GameObject prefab;
    }
}

