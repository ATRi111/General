using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Character
{
    public enum EInvokeMode
    {
        Update,
        FixedUpdate,
    }

    public class InputTuple<T> where T : InputEvent, new()
    {
        public T input;
        private readonly Dictionary<EInvokeMode, UnityEvent<T>> actions;

        public InputTuple(string axesName)
        {
            actions = new Dictionary<EInvokeMode, UnityEvent<T>>();
            input = new T() { axesName = axesName };
            foreach (EInvokeMode mode in Enum.GetValues(typeof(EInvokeMode)))
            {
                actions.Add(mode, new UnityEvent<T>());
            }
        }

        public void Invoke(EInvokeMode mode)
            => actions[mode].Invoke(input);

        public void Add(UnityAction<T> action, EInvokeMode mode)
            => actions[mode].AddListener(action);

        public void Remove(UnityAction<T> action, EInvokeMode mode)
            => actions[mode].RemoveListener(action);
    }
}