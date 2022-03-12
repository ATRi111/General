using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-200)]
public class ServiceLocator : Singleton<ServiceLocator>
{
    private readonly Dictionary<Type, Service> serviceDict = new Dictionary<Type, Service>();

    /// <summary>
    /// ��ȡ����ΪT�ķ���
    /// </summary>
    public T GetService<T>() where T : Service
    {
        Type type = typeof(T);
        return serviceDict[type] as T;
    }

    public void Register(Service service)
    {
        Type type = service.GetType();
        if (serviceDict.ContainsKey(type))
        {
            Debug.Log($"�������õĽű����޸��ˣ���������Ϊ{type}");
            serviceDict[type] = service;
        }
        else
            serviceDict.Add(type, service);
    }
}
