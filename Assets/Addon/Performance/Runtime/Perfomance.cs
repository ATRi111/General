using UnityEngine;
using UnityEngine.Playables;

namespace Performance
{
    [RequireComponent(typeof(PlayableDirector))]
    public class PerfomancePlayer : MonoBehaviour
    {
        public PlayableDirector Director { get; private set; }

        [SerializeField]
        private PerformanceData data;

        private void Awake()
        {
            Director = GetComponent<PlayableDirector>();

            if (Director.playOnAwake)
            {
                Debug.LogWarning($"{gameObject}的playOnAwake不应被设为true");
                Director.playOnAwake = false;
            }
        }

        
    }
}