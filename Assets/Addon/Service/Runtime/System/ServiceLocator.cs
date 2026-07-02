using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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

    /// <summary>
    /// 服务定位器外观层。
    /// 内部按"作用域"组织：1 个全局 ServiceManager + 每个加载场景 1 个 SceneServiceManager。
    /// </summary>
    public static class ServiceLocator
    {
        // TODO: 作用域设计稳定后，重新引入以下两个事件
        ///// <summary>
        ///// 服务初始化（参数：刚初始化好的服务）
        ///// </summary>
        //public static event UnityAction<Service> ServiceInit
        //{
        //    add { serviceInit += value; }
        //    remove { serviceInit -= value; }
        //}
        //public static event UnityAction<Scene, Service> SceneServiceInit
        //{
        //    add { sceneServiceInit += value; }
        //    remove { sceneServiceInit -= value; }
        //}
        //private static UnityAction<Service> serviceInit;
        //private static UnityAction<Scene, Service> sceneServiceInit;

        /// <summary>全局作用域（跨场景持续存在）</summary>
        internal static readonly ServiceManager Global = new ServiceManager(isGlobal: true, scene: default);

        /// <summary>各场景作用域，key = Scene.handle</summary>
        private static readonly Dictionary<int, ServiceManager> scenes = new Dictionary<int, ServiceManager>();

        static ServiceLocator()
        {
            // 订阅场景卸载，自动清理对应的 ServiceManager
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        /// <summary>获取一个类型的服务（先当前激活场景，再全局）</summary>
        public static T Get<T>() where T : class, IService
            => Get(typeof(T)) as T;

        public static Service Get(Type type)
        {
            // 1. 先查当前激活场景
            Scene active = SceneManager.GetActiveScene();
            if (IsRealScene(active))
            {
                if (TryGetSceneManager(active.handle, out var mgr)
                    && mgr.TryGet(type, out Service ret))
                    return ret;
            }
            // 2. 兜底：全局
            if (Global.TryGet(type, out Service g))
                return g;
            throw new InvalidOperationException($"服务{type}未注册");
        }

        /// <summary>显式从指定作用域获取</summary>
        public static T Get<T>(Scene scope) where T : class, IService
            => Get(scope, typeof(T)) as T;

        public static Service Get(Scene scope, Type type)
        {
            ServiceManager mgr = GetManager(scope);
            return mgr.Get(type);
        }

        /// <summary>显式获取全局服务</summary>
        public static T GetGlobal<T>() where T : class, IService
            => Global.Get(typeof(T)) as T;

        /// <summary>获取 Service.Awake 路由用的 ServiceManager</summary>
        internal static ServiceManager GetManager(Scene scene)
        {
            if (IsGlobalScene(scene))
                return Global;
            return GetOrCreateSceneManager(scene);
        }

        internal static void Register(Service service, EConflictSolution solution)
        {
            ServiceManager mgr = GetManager(service.gameObject.scene);
            mgr.Register(service, solution);
        }

        internal static void Unregister(Service service)
        {
            // 用 RegisterType 不一定就能反查 manager（同一类型在多个作用域都有时），
            // 因此优先按 scene 路由；scene 失效则尝试 Global。
            Scene scene = service.gameObject.scene;
            ServiceManager mgr = IsGlobalScene(scene) ? Global : GetOrCreateSceneManager(scene);
            mgr.Unregister(service);
        }

        public static void Clear()
        {
            Global.Clear();
            // 复制避免遍历时修改
            var allSceneMgrs = new List<ServiceManager>(scenes.Values);
            scenes.Clear();
            for (int i = 0; i < allSceneMgrs.Count; i++)
                allSceneMgrs[i].Clear();
        }

        // --- 内部辅助 ---

        private static bool TryGetSceneManager(int handle, out ServiceManager mgr)
        {
            if (scenes.TryGetValue(handle, out mgr))
                return true;
            mgr = null;
            return false;
        }

        private static ServiceManager GetOrCreateSceneManager(Scene scene)
        {
            if (!scenes.TryGetValue(scene.handle, out var mgr))
            {
                mgr = new ServiceManager(isGlobal: false, scene: scene);
                scenes.Add(scene.handle, mgr);
            }
            return mgr;
        }

        private static void OnSceneUnloaded(Scene s)
        {
            if (scenes.TryGetValue(s.handle, out var mgr))
            {
                scenes.Remove(s.handle);
                mgr.DisposeAll();
            }
        }

        /// <summary>是否属于全局（DontDestroyOnLoad）场景</summary>
        private static bool IsGlobalScene(Scene s)
        {
            return !s.IsValid() || s.name == "DontDestroyOnLoad";
        }

        /// <summary>是否为一个"真实可查"的场景（非 default、非 DontDestroyOnLoad）</summary>
        private static bool IsRealScene(Scene s)
        {
            return s.IsValid() && s.name != "DontDestroyOnLoad";
        }
    }
}
