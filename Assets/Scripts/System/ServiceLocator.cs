using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-200)]
public class ServiceLocator : Singleton<ServiceLocator>
{
    private readonly Dictionary<EService, Service> serviceDict = new Dictionary<EService, Service>();

    /// <summary>
    /// 获取一个服务
    /// </summary>
    /// <returns>如果要将获取的服务赋值给其父类，泛型仍应指定为该服务的类而不是其父类</returns>
    public T GetService<T>(EService eService) where T : Service
    {
        if (!serviceDict.ContainsKey(eService))
        {
            Debug.LogWarning($"服务未被添加到服务定位器中,服务名称为{eService}");
            return null;
        }

        Service service = serviceDict[eService];
        if (!(service is T))
        {
            Debug.LogWarning($"无法获取正确的服务，服务的类型为{service.GetType().Name}");
            return null;
        }
        return (T)serviceDict[eService];
    }

    public void AddService(EService eService, Service service)
    {
        serviceDict.Add(eService, service);
    }
}
