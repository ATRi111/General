using System;
using System.Collections.Generic;
using UnityEngine;


public class Service : MonoBehaviour
{
    private static readonly Dictionary<Type, Service> serviceDict = new Dictionary<Type, Service>();

    /// <typeparam name="T">���Ի�ȡĳ�����࣬�ٽ��丳ֵ�����࣬��������£�TӦָ��Ϊ����</typeparam>
    public static T Get<T>() where T : Service
    {
        Type type = typeof(T);
        if (!serviceDict.ContainsKey(type))
            Debug.LogError($"û������Ϊ{type}�ķ���");
        return serviceDict[type] as T;
    }

    protected static void Register(Service service)
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

    /// <summary>
    /// ���Ҫ��Awake�л�ȡ��������ע��ű�ִ��˳��
    /// </summary>
    protected virtual void Awake()
    {
        Service.Register(this);
    }
}
