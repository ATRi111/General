using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, Service> serviceDict = new Dictionary<Type, Service>();

    /// <typeparam name="T">可以获取某个子类，再将其赋值给父类，这种情况下，T应指定为子类</typeparam>
    public static T GetService<T>() where T : Service
    {
        Type type = typeof(T);
        if (!serviceDict.ContainsKey(type)) 
            Debug.LogError($"没有类型为{type}的服务");
        return serviceDict[type] as T;
    }

    public static void Register(Service service)
    {
        Type type = service.GetType();
        if (serviceDict.ContainsKey(type))
        {
            Debug.Log($"服务引用的脚本被修改了，服务类型为{type}");
            serviceDict[type] = service;
        }
        else
            serviceDict.Add(type, service);
    }
}
