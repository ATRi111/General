using System;
using System.Collections.Generic;
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
        /// 服务初始化，参数：刚初始化好的服务
        /// </summary>
        public static UnityAction<Service> ServiceInit;

        internal static readonly Dictionary<Type, Service> serviceDict = new Dictionary<Type, Service>();

        /// <summary>
        /// 获取一个类型的服务
        /// </summary>
        /// <typeparam name="T">此参数对应Service类的RegisterType</typeparam>
        public static T Get<T>() where T : class, IService
            => Get(typeof(T)) as T;

        public static Service Get(Type type)
        {
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
        /// 解决服务冲突
        /// </summary>
        /// <returns>是否要将新服务加入字典</returns>
        private static bool SolveConflict(Service oldService, Service newService, EConflictSolution solution)
        {
            if (oldService == newService)
                return false;

            bool ret = false;
            Debugger.LogWarning($"服务发生冲突,旧服务{oldService.Informantion};\n新服务{newService.Informantion};解决方式为{solution}", EMessageType.System);
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