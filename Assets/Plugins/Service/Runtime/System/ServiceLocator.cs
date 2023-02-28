using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Services
{
    /// <summary>
    /// ͬʱ���ڶ��ͬ���ͷ���ʱ��������ͻ�Ĳ���
    /// </summary>
    public enum EConflictSolution
    {
        /// <summary>
        /// ���پɷ���
        /// </summary>
        DestroyOld,
        /// <summary>
        /// �����·���
        /// </summary>
        DestroyNew,
        /// <summary>
        /// ȡ���ɷ����ע�ᣬ�������پɷ���
        /// </summary>
        UnregisterOld,
        /// <summary>
        /// ȡ���·����ע�ᣬ���������·���
        /// </summary>
        UnregisterNew,
    }

    public static class ServiceLocator
    {
        /// <summary>
        /// �����ʼ�����������ճ�ʼ���õķ���
        /// </summary>
        public static UnityAction<Service> ServiceInit;

        internal static readonly Dictionary<Type, Service> serviceDict = new Dictionary<Type, Service>();

        /// <summary>
        /// ��ȡһ�����͵ķ���
        /// </summary>
        /// <typeparam name="T">�˲��������Ǽ̳���IService�Ľӿ����ͣ��˽ӿڴ�������ͬ�����ܵ�һ�����</typeparam>
        internal static T Get<T>() where T : class, IService
            => Get(typeof(T)) as T;

        internal static Service Get(Type type)
        {
            if (!IService.ExtendsIService(type))
            {
                Debugger.LogWarning("Ӧ��ʹ��ֱ�Ӽ̳�IService�Ľӿ�����Ϊ����", EMessageType.System);
                return null;
            }

            if (TryGet(type, out Service ret))
                return ret;
            return ret;
        }

        internal static void Register(Service service, EConflictSolution solution = EConflictSolution.DestroyNew)
        {
            Type type = service.RegisterType;
            if (TryGet(type, out Service oldService))
            {
                if (SolveConflict(oldService, service, solution))
                    serviceDict.Add(type, service);
            }
            else
                serviceDict.Add(type, service);
        }

        internal static void Unregister(Service service)
        {
            Type type = service.RegisterType;
            if (!TryGet(type, out Service ret))
                return;
            if (ret != service)
                return;

            serviceDict.Remove(type);
        }

        private static bool TryGet(Type type, out Service ret)
        {
            if (!serviceDict.ContainsKey(type))
            {
                ret = null;
                return false;
            }

            ret = serviceDict[type];
            return true;
        }

        /// <summary>
        /// ��������ͻ
        /// </summary>
        /// <returns>�Ƿ�Ҫ���·�������ֵ�</returns>
        private static bool SolveConflict(Service oldService, Service newService, EConflictSolution solution)
        {
            if (oldService == newService)
                return false;

            bool ret = false;
            Debugger.LogWarning($"��������ͻ,�ɷ���{oldService.Informantion};\n�·���{newService.Informantion};�����ʽΪ{solution}", EMessageType.System);
            switch (solution)
            {
                case EConflictSolution.DestroyOld:
                    oldService.Destroy();
                    ret = true;
                    break;
                case EConflictSolution.DestroyNew:
                    newService.Destroy();
                    break;
                case EConflictSolution.UnregisterOld:
                    Unregister(oldService);
                    ret = true;
                    break;
                case EConflictSolution.UnregisterNew:
                    break;
            }
            return ret;
        }
    }
}