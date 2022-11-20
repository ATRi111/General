using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    /// <summary>
    /// 同时存在多个同类型服务时，处理冲突的策略
    /// </summary>
    public enum EConflictSolution
    {
        /// <summary>
        /// 销毁旧服务
        /// </summary>
        DestroyOld,
        /// <summary>
        /// 销毁新服务
        /// </summary>
        DestroyNew,
        /// <summary>
        /// 取消旧服务的注册，但不销毁旧服务
        /// </summary>
        UnregisterOld,
        /// <summary>
        /// 取消新服务的注册，但不销毁新服务
        /// </summary>
        UnregisterNew,
    }

    public static class ServiceLocator
    {
        /// <summary>
        /// 用于与Service同时初始化的脚本确定获取Service的时机
        /// </summary>
        public static UnityAction<Service> ServiceInit;

        internal static readonly Dictionary<Type, Service> serviceDict = new Dictionary<Type, Service>();

        /// <summary>
        /// 获取一个类型的服务
        /// </summary>
        /// <typeparam name="T">试图获取某类型的服务时，不一定以那个类的Type为参数，要看其RegisterType</typeparam>
        internal static T Get<T>() where T : Service
            => Get(typeof(T)) as T;

        internal static Service Get(Type type)
        {
            TryGet(type, out Service ret);
            return ret;
        }

        internal static void Register(Service service, EConflictSolution solution = EConflictSolution.DestroyNew)
        {
            Type type = service.RegisterType;
            if (TryGet(type, out Service oldService))
            {
                if(SolveConflict(oldService, service, solution))
                    serviceDict.Add(type, service);
            }
            else
                serviceDict.Add(type, service);
        }

        internal static void Unregister(Service service)
        {
            TryRemove(service.RegisterType, service);
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

        private static bool TryRemove(Type type,Service service)
        {
            if (!TryGet(type, out Service ret))
                return false;
            if (ret != service)
                return false;

            serviceDict.Remove(type);
            return true;
        }

        /// <summary>
        /// 解决服务冲突
        /// </summary>
        /// <returns>是否要将新服务加入字典</returns>
        private static bool SolveConflict(Service oldService,Service newService, EConflictSolution solution)
        {
            if (oldService == newService)
                return false;

            bool ret = false;
            Debugger.LogWarning($"服务发生冲突,旧服务{oldService.Informantion};\n新服务{newService.Informantion};解决方式为{solution}", EMessageType.System);
            switch(solution)
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