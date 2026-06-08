using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Services.Event
{
    internal class EventSystemCore
    {
        internal readonly Dictionary<EEvent, Type> typeDict;
        internal readonly Dictionary<EEvent, Delegate> eventDict;

        internal EventSystemCore()
        {
            typeDict = new Dictionary<EEvent, Type>();
            eventDict = new Dictionary<EEvent, Delegate>();
        }

        private bool Check(EEvent eEvent, Type methodType, bool logIfMissing = true)
        {
            if (!typeDict.ContainsKey(eEvent))
            {
                if (logIfMissing)
                    Debugger.LogWarning($"不存在名为{eEvent}的事件", EMessageType.System);
                return false;
            }
            if (typeDict[eEvent] != methodType)
            {
                Debugger.LogWarning($"响应方法的类型不符合事件所要求的类型,事件名为{eEvent}", EMessageType.System);
                return false;
            }
            return true;
        }

        private bool CheckOrCreate(EEvent eEvent, Type methodType)
        {
            if (typeDict.ContainsKey(eEvent))
                return Check(eEvent, methodType, false);
            CreateEvent(eEvent, methodType);
            return true;
        }

        private void CreateEvent(EEvent eEvent, Type type)
        {
            if (!type.IsSubclassOf(typeof(Delegate)))
            {
                Debugger.LogWarning($"{type}不是Delegate的子类", EMessageType.System);
                return;
            }
            if (eventDict.ContainsKey(eEvent))
            {
                Debugger.LogWarning($"名为{eEvent}的事件已存在", EMessageType.System);
                return;
            }
            typeDict.Add(eEvent, type);
            eventDict.Add(eEvent, null);
        }

        public void AddListener(EEvent eEvent, UnityAction callBack)
        {
            if (CheckOrCreate(eEvent, callBack.GetType()))
                eventDict[eEvent] = eventDict[eEvent] as UnityAction + callBack;
        }
        public void AddListener<T1>(EEvent eEvent, UnityAction<T1> callBack)
        {
            if (CheckOrCreate(eEvent, callBack.GetType()))
                eventDict[eEvent] = eventDict[eEvent] as UnityAction<T1> + callBack;
        }
        public void AddListener<T1, T2>(EEvent eEvent, UnityAction<T1, T2> callBack)
        {
            if (CheckOrCreate(eEvent, callBack.GetType()))
                eventDict[eEvent] = eventDict[eEvent] as UnityAction<T1, T2> + callBack;
        }
        public void AddListener<T1, T2, T3>(EEvent eEvent, UnityAction<T1, T2, T3> callBack)
        {
            if (CheckOrCreate(eEvent, callBack.GetType()))
                eventDict[eEvent] = eventDict[eEvent] as UnityAction<T1, T2, T3> + callBack;
        }

        public void RemoveListener(EEvent eEvent, UnityAction callBack)
        {
            if (Check(eEvent, callBack.GetType()))
                eventDict[eEvent] = eventDict[eEvent] as UnityAction - callBack;
        }
        public void RemoveListener<T1>(EEvent eEvent, UnityAction<T1> callBack)
        {
            if (Check(eEvent, callBack.GetType()))
                eventDict[eEvent] = eventDict[eEvent] as UnityAction<T1> - callBack;
        }
        public void RemoveListener<T1, T2>(EEvent eEvent, UnityAction<T1, T2> callBack)
        {
            if (Check(eEvent, callBack.GetType()))
                eventDict[eEvent] = eventDict[eEvent] as UnityAction<T1, T2> - callBack;
        }
        public void RemoveListener<T1, T2, T3>(EEvent eEvent, UnityAction<T1, T2, T3> callBack)
        {
            if (Check(eEvent, callBack.GetType()))
                eventDict[eEvent] = eventDict[eEvent] as UnityAction<T1, T2, T3> - callBack;
        }

        public void Invoke(EEvent eEvent)
        {
            if (Check(eEvent, typeof(UnityAction)))
                (eventDict[eEvent] as UnityAction)?.Invoke();
        }
        public void Invoke<T1>(EEvent eEvent, T1 arg1)
        {
            if (Check(eEvent, typeof(UnityAction<T1>)))
                (eventDict[eEvent] as UnityAction<T1>)?.Invoke(arg1);
        }
        public void Invoke<T1, T2>(EEvent eEvent, T1 arg1, T2 arg2)
        {
            if (Check(eEvent, typeof(UnityAction<T1, T2>)))
                (eventDict[eEvent] as UnityAction<T1, T2>)?.Invoke(arg1, arg2);
        }
        public void Invoke<T1, T2, T3>(EEvent eEvent, T1 arg1, T2 arg2, T3 arg3)
        {
            if (Check(eEvent, typeof(UnityAction<T1, T2, T3>)))
                (eventDict[eEvent] as UnityAction<T1, T2, T3>)?.Invoke(arg1, arg2, arg3);
        }
    }
}