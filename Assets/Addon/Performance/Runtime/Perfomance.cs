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
                Debug.LogWarning($"{gameObject}��playOnAwake��Ӧ����Ϊtrue");
                Director.playOnAwake = false;
            }
        }

        
    }
}