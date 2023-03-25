using UnityEngine;
using UnityEngine.Events;

namespace Services.Event
{
    [DefaultExecutionOrder(-100)]
    public sealed class EventSystem : Service, IEventSystem
    {
        private EventSystemCore core;

        protected override void Awake()
        {
            base.Awake();
            core = new EventSystemCore();
        }

        public void AddListener(EEvent eEvent, UnityAction callBack)
            => core.AddListener(eEvent, callBack);
        public void AddListener<T1>(EEvent eEvent, UnityAction<T1> callBack)
            => core.AddListener(eEvent, callBack);
        public void AddListener<T1, T2>(EEvent eEvent, UnityAction<T1, T2> callBack)
            => core.AddListener(eEvent, callBack);
        public void AddListener<T1, T2, T3>(EEvent eEvent, UnityAction<T1, T2, T3> callBack)
             => core.AddListener(eEvent, callBack);

        public void RemoveListener(EEvent eEvent, UnityAction callBack)
            => core.RemoveListener(eEvent, callBack);
        public void RemoveListener<T1>(EEvent eEvent, UnityAction<T1> callBack)
            => core.RemoveListener(eEvent, callBack);
        public void RemoveListener<T1, T2>(EEvent eEvent, UnityAction<T1, T2> callBack)
            => core.RemoveListener(eEvent, callBack);
        public void RemoveListener<T1, T2, T3>(EEvent eEvent, UnityAction<T1, T2, T3> callBack)
            => core.RemoveListener(eEvent, callBack);

        public void Invoke(EEvent eEvent)
            => core.Invoke(eEvent);
        public void Invoke<T1>(EEvent eEvent, T1 arg1)
            => core.Invoke(eEvent, arg1);
        public void Invoke<T1, T2>(EEvent eEvent, T1 arg1, T2 arg2)
            => core.Invoke(eEvent, arg1, arg2);
        public void Invoke<T1, T2, T3>(EEvent eEvent, T1 arg1, T2 arg2, T3 arg3)
            => core.Invoke(eEvent, arg1, arg2, arg3);
    }
}