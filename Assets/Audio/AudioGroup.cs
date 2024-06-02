using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Services.Audio
{
    [CreateAssetMenu(menuName = "Services/AudioSourceGroup")]
    public class AudioGroup : ScriptableObject
    {
        [SerializeField]
        private List<GameObject> prefabs;

        public GameObject RandomPrefab()
        {
            int r = RandomTool.RandomInt(0, prefabs.Count);
            return prefabs[r];
        }

        public GameObject GetPrefab(int index)
        {
            if(index >=0 && index < prefabs.Count)
                return prefabs[index];
            return RandomPrefab();
        }
    }
}