using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyStateMachine
{
    /// <summary>
    /// ����index��State��ӳ��
    /// </summary>
    public class StateMapper
    {
        protected Dictionary<int, State> stateDict = new Dictionary<int, State>();
        internal StateMachine stateMachine;

        public State GetState(int enumIndex)
        {
            if (stateDict.ContainsKey(enumIndex))
                return stateDict[enumIndex];
            Debug.LogWarning($"δ����{enumIndex}״̬");
            return null;
        }

        /// <summary>
        /// ���һ��״̬����Ϊ������µ�index
        /// </summary>
        public void AddState(int stateIndex, State state)
        {
            if (!stateDict.ContainsKey(stateIndex))
            {
                stateDict.Add(stateIndex, state);
                state.enumIndex = stateIndex;
                state.stateMachine = stateMachine;
            }
        }

        /// <summary>
        /// �������״̬��Ҫ�����ȷ����index
        /// </summary>
        public void AddStates(params State[] states)
        {
            foreach (State state in states)
            {
                AddState(state.enumIndex, state);
            }
        }

        /// <summary>
        /// ͨ��State���ͺ�ö�����Ϳ��ٴ���״̬����������״̬��Ϊ��ͬ����;����
        /// </summary>
        public void AddSameStates<T, E>() where T : State where E : Enum
        {
            foreach (E value in Enum.GetValues(typeof(E)))
            {
                T state = Activator.CreateInstance<T>();
                AddState(Convert.ToInt32(value), state);
            }
        }
    }
}