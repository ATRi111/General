using System.Collections.Generic;
using UnityEngine;

namespace StateMathine
{
    public class StateManager : Service
    {
        private Dictionary<EState, State> stateDict;

        public void Start()
        {
            stateDict.Add(EState.Default, new State());
        }

        public State GetState(EState eState)
        {
            if (stateDict.ContainsKey(eState))
                return stateDict[eState];
            Debug.LogWarning($"״̬�����ڣ�״̬��Ϊ{eState}");
            return null;
        }
    }
}

