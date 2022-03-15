using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, Service> serviceDict = new Dictionary<Type, Service>();

    /// <summary>
    /// 获取类型为T的服务
    /// </summary>
    public static T GetService<T>() where T : Service
    {
        Type type = typeof(T);
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
