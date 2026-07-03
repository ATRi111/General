using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Services
{
    /// <summary>
    /// 服务定位器
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>服务的作用域按scene划分，以scene的handle为key，其中0表示全局作用域</summary>
        private static readonly Dictionary<int, ServiceManager> handles = new Dictionary<int, ServiceManager>();

        static ServiceLocator()
        {
            // 订阅场景卸载，自动清理对应的 ServiceManager
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        public static Service Get(int handle, Type type)
        {
            if (handles.TryGetValue(handle, out var mgr))
                return mgr.Get(type);
            throw new InvalidOperationException($"服务{type}未注册(作用域 handle={handle})");
        }

        public static T Get<T>() where T : class, IService
            => Get<T>(0);
        public static Service Get(Type type)
            => Get(0, type);
        public static T Get<T>(int handle) where T : class, IService
            => Get(handle, typeof(T)) as T;
        public static T Get<T>(Scene scope) where T : class, IService
            => Get(scope.handle, typeof(T)) as T;
        public static Service Get(Scene scope, Type type)
            => Get(scope.handle, type);

        /// <summary>Service.Awake 路由用的 ServiceManager</summary>
        internal static void Register(Service service, Action<Service, Service> conflictHandler)
        {
            int handle = service.Handle;
            if (!handles.TryGetValue(handle, out var mgr))
            {
                mgr = new ServiceManager(handle);
                handles.Add(handle, mgr);
            }
            mgr.Register(service, conflictHandler);
        }

        internal static void Unregister(Service service)
        {
            int handle = service.Handle;
            if (handles.TryGetValue(handle, out var mgr))
                mgr.Unregister(service);
        }

        private static void OnSceneUnloaded(Scene s)
        {
            // 场景卸载时，所有该场景 Service 的 OnDestroy 已先于本回调触发
            // 并自行 Unregister 过；这里只需把字典项移除即可
            handles.Remove(s.handle);
        }
    }
}
