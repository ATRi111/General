using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Services.Performance
{
    [RequireComponent(typeof(PlayableDirector))]
    public class PerformancePlayerBase : MonoBehaviour
    {
        private IPerformanceManager manager;

        public PlayableDirector Director { get; private set; }
        public List<bool> useDolls;

        protected virtual void Awake()
        {
            Director = GetComponent<PlayableDirector>();
            manager = ServiceLocator.Get<IPerformanceManager>();

            if (Director.playOnAwake)
            {
                Debug.LogWarning($"{gameObject}的playOnAwake不应被设为true");
                Director.playOnAwake = false;
            }
        }

        protected virtual void Register_Play()
        {
            for (int i = 0; i <= useDolls.Count; i++)
            {
                if (useDolls[i])
                    manager.UseDoll?.Invoke((EDollType)i);
            }
        }

        protected virtual void Register_Pause()
        {

        }

        protected virtual void Register_Stop()
        {
            for (int i = 0; i <= useDolls.Count; i++)
            {
                if (useDolls[i])
                    manager.StopUseDoll?.Invoke((EDollType)i);
            }
        }
    }
}