using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可通过Get获取继承此类的子类对象，每个类的对象应当是唯一的
/// </summary>
public class Service : MonoBehaviour
{
    private static readonly Dictionary<Type, Service> serviceDict = new Dictionary<Type, Service>();

    /// <typeparam name="T">可以获取某个子类，再将其赋值给父类，这种情况下，T应指定为子类</typeparam>
    public static T Get<T>() where T : Service
    {
        Type type = typeof(T);
        if (!serviceDict.ContainsKey(type))
        {
            Debug.LogWarning($"没有类型为{type}的服务");
            return null;
        }
        return serviceDict[type] as T;
    }

    public static void Register(Service service)
    {
        Type type = service.GetType();
        if (serviceDict.ContainsKey(type))
        {
            Debug.LogWarning($"服务引用的脚本被修改了，服务类型为{type}");
            serviceDict[type] = service;
        }
        else
            serviceDict.Add(type, service);
    }

    /// <summary>
    /// 再脚本顺序不受控制的情况下，不要在Awake中获取其他服务
    /// </summary>
    protected virtual void Awake()
    {
        Service.Register(this);
    }
}
