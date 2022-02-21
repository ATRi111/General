using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-200)]
public class ServiceLocator : Singleton<ServiceLocator>
{
    private readonly Dictionary<EService, Service> serviceDict = new Dictionary<EService, Service>();

    /// <summary>
    /// ��ȡһ������
    /// </summary>
    /// <returns>���Ҫ����ȡ�ķ���ֵ���丸�࣬������Ӧָ��Ϊ�÷������������丸��</returns>
    public T GetService<T>(EService eService) where T : Service
    {
        if (!serviceDict.ContainsKey(eService))
        {
            Debug.LogWarning($"����δ����ӵ�����λ����,��������Ϊ{eService}");
            return null;
        }

        Service service = serviceDict[eService];
        if (!(service is T))
        {
            Debug.LogWarning($"�޷���ȡ��ȷ�ķ��񣬷��������Ϊ{service.GetType().Name}");
            return null;
        }
        return (T)serviceDict[eService];
    }

    public void AddService(EService eService, Service service)
    {
        serviceDict.Add(eService, service);
    }
}
