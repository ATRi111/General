using System;
using System.Collections.Generic;
using UnityEngine;


public class Service : MonoBehaviour
{
    private static readonly Dictionary<Type, Service> serviceDict = new Dictionary<Type, Service>();

    /// <typeparam name="T">可以获取某个子类，再将其赋值给父类，这种情况下，T应指定为子类</typeparam>
    public static T Get<T>() where T : Service
    {
        Type type = typeof(T);
        if (!serviceDict.ContainsKey(type))
            Debug.LogError($"没有类型为{type}的服务");
        return serviceDict[type] as T;
    }

    protected static void Register(Service service)
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

    /// <summary>
    /// 如果要在Awake中获取其他服务，注意脚本执行顺序
    /// </summary>
    protected virtual void Awake()
    {
        Service.Register(this);
    }
}
