using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Services
{
    [RequireComponent(typeof(PlayableDirector))]
    public class PerformancePlayerBase : MonoBehaviour
    {
        private PerformanceManagerBase manager;

        public PlayableDirector Director { get; private set; }
        public EDollType useDollFlags;
        private int[] enumValues;

        protected virtual void Awake()
        {
            Director = GetComponent<PlayableDirector>();
            manager = ServiceLocator.Get<PerformanceManagerBase>();
            Array temp = Enum.GetValues(typeof(EDollType));
            enumValues = new int[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                enumValues[i] = temp.GetValue(i).GetHashCode();
            }

            if (Director.playOnAwake)
            {
                Debug.LogWarning($"{gameObject}的playOnAwake不应被设为true");
                Director.playOnAwake = false;
            }
        }

        protected virtual void Register_Play()
        {
            for (int i = 0; i <= enumValues.Length; i++ )
            {
                if ((enumValues[i] & (int)useDollFlags) != 0)
                {
                    manager.UseDoll?.Invoke((EDollType)enumValues[i]);
                }
            }
        }

        protected virtual void Register_Pause()
        {

        }

        protected virtual void Register_Stop()
        {
            for(int i = 0; i <= enumValues.Length; i++ )
            {
                if ((enumValues[i] & (int)useDollFlags) != 0)
                {
                    manager.StopUseDoll?.Invoke((EDollType)enumValues[i]);
                }
            }
        }
    }
}