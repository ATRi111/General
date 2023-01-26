using UnityEngine.Events;

namespace Services.Event
{
    public interface IEventSystem : IService
    {
        void AddListener(EEvent eEvent, UnityAction callBack);
        void AddListener<T1, T2, T3>(EEvent eEvent, UnityAction<T1, T2, T3> callBack);
        void AddListener<T1, T2>(EEvent eEvent, UnityAction<T1, T2> callBack);
        void AddListener<T1>(EEvent eEvent, UnityAction<T1> callBack);
        void Invoke(EEvent eEvent);
        void Invoke<T1, T2, T3>(EEvent eEvent, T1 arg1, T2 arg2, T3 arg3);
        void Invoke<T1, T2>(EEvent eEvent, T1 arg1, T2 arg2);
        void Invoke<T1>(EEvent eEvent, T1 arg1);
        void RemoveListener(EEvent eEvent, UnityAction callBack);
        void RemoveListener<T1, T2, T3>(EEvent eEvent, UnityAction<T1, T2, T3> callBack);
        void RemoveListener<T1, T2>(EEvent eEvent, UnityAction<T1, T2> callBack);
        void RemoveListener<T1>(EEvent eEvent, UnityAction<T1> callBack);
    }
}