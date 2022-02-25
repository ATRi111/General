using System;
using System.Collections.Generic;
using UnityEngine;
using static EEvent;

[DefaultExecutionOrder(-100)]
public class EventSystem : Service
{
    private readonly Dictionary<EEvent, Type> typeDict = new Dictionary<EEvent, Type>()
    {
        {BeforeLoadScene,typeof(Action<int>) },
        {AfterLoadScene,typeof(Action<int>) },
    };
    private Dictionary<EEvent, Delegate> eventDict;

    protected override void BeforeRegister()
    {
        eService = EService.EventSystem;
    }

    private void Start()
    {
        eventDict = new Dictionary<EEvent, Delegate>();
        foreach (EEvent key in typeDict.Keys)
        {
            eventDict.Add(key, null);
        }
    }

    private bool TypeCheck(EEvent eEvent, Type methodType)
    {
        if (typeDict[eEvent] != methodType)
        {
            Debug.LogWarning($"响应方法的类型不符合事件所要求的类型,事件名为{eEvent}");
            return false;
        }
        return true;
    }

    public void Register(EEvent eEvent, Action callBack)
    {
        if (TypeCheck(eEvent, callBack.GetType()))
            eventDict[eEvent] = (Action)eventDict[eEvent] + callBack;
    }
    public void Register<T1>(EEvent eEvent, Action<T1> callBack)
    {
        if (TypeCheck(eEvent, callBack.GetType()))
            eventDict[eEvent] = (Action<T1>)eventDict[eEvent] + callBack;
    }
    public void Register<T1, T2>(EEvent eEvent, Action<T1, T2> callBack)
    {
        if (TypeCheck(eEvent, callBack.GetType()))
            eventDict[eEvent] = (Action<T1, T2>)eventDict[eEvent] + callBack;
    }

    public void Unregister(EEvent eEvent, Action callBack)
    {
        if (TypeCheck(eEvent, callBack.GetType()))
            eventDict[eEvent] = (Action)eventDict[eEvent] - callBack;
    }
    public void Unregister<T1>(EEvent eEvent, Action<T1> callBack)
    {
        if (TypeCheck(eEvent, callBack.GetType()))
            eventDict[eEvent] = (Action<T1>)eventDict[eEvent] - callBack;
    }
    public void Unregister<T1, T2>(EEvent eEvent, Action<T1, T2> callBack)
    {
        if (TypeCheck(eEvent, callBack.GetType()))
            eventDict[eEvent] = (Action<T1, T2>)eventDict[eEvent] - callBack;
    }

    public void ActivateEvent(EEvent eEvent)
    {
        if (TypeCheck(eEvent, typeof(Action)))
            (eventDict[eEvent] as Action)?.Invoke();
    }
    public void ActivateEvent<T1>(EEvent eEvent, T1 arg1)
    {
        if (TypeCheck(eEvent, typeof(Action<T1>)))
            (eventDict[eEvent] as Action<T1>)?.Invoke(arg1);
    }
    public void ActivateEvent<T1, T2>(EEvent eEvent, T1 arg1, T2 arg2)
    {
        if (TypeCheck(eEvent, typeof(Action<T1, T2>)))
            (eventDict[eEvent] as Action<T1, T2>)?.Invoke(arg1, arg2);
    }
}
