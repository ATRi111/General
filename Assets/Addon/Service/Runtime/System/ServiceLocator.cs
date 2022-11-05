using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    public static class ServiceLocator
    {
        /// <summary>
        /// ��0�����е�����ͨ��ֱ�ӻ�ȡService������Ҫ���ô��¼�
        /// </summary>
        public static UnityAction<Service> ServiceInit;

        internal static readonly Dictionary<Type, Service> serviceDict = new Dictionary<Type, Service>();

        /// <summary>
        /// ��ȡһ������
        /// </summary>
        /// <typeparam name="T">�ڶ��������ѡ��һ��ʱ��TӦָ��Ϊһ����������ǹ�ͬ�ĸ���</typeparam>
        internal static T Get<T>() where T : Service
            => Get(typeof(T)) as T;

        internal static Service Get(Type type)
        {
            if (!serviceDict.ContainsKey(type))
            {
                Debug.LogWarning($"���񲻴��ڣ���������Ϊ{type}");
                return null;
            }
            return serviceDict[type];
        }

        internal static void Register(Service service)
        {
            Type type = service.RegisterType;
            if (serviceDict.ContainsKey(type))
            {
                Debug.LogWarning($"�������õĽű����޸��ˣ���������Ϊ{type}");
                serviceDict[type] = service;
            }
            else
                serviceDict.Add(type, service);
        }
    }
}