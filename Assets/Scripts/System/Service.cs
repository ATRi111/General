using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ͨ��Get��ȡ�̳д�����������ÿ����Ķ���Ӧ����Ψһ��
/// </summary>
public class Service : MonoBehaviour
{
    private static readonly Dictionary<Type, Service> serviceDict = new Dictionary<Type, Service>();

    /// <typeparam name="T">���Ի�ȡĳ�����࣬�ٽ��丳ֵ�����࣬��������£�TӦָ��Ϊ����</typeparam>
    public static T Get<T>() where T : Service
    {
        Type type = typeof(T);
        if (!serviceDict.ContainsKey(type))
        {
            Debug.LogWarning($"û������Ϊ{type}�ķ���");
            return null;
        }
        return serviceDict[type] as T;
    }

    public static void Register(Service service)
    {
        Type type = service.GetType();
        if (serviceDict.ContainsKey(type))
        {
            Debug.LogWarning($"�������õĽű����޸��ˣ���������Ϊ{type}");
            serviceDict[type] = service;
        }
        else
            serviceDict.Add(type, service);
    }

    /// <summary>
    /// �ٽű�˳���ܿ��Ƶ�����£���Ҫ��Awake�л�ȡ��������
    /// </summary>
    protected virtual void Awake()
    {
        Service.Register(this);
    }
}
