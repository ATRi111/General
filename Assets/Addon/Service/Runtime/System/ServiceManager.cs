using System;
using System.Collections.Generic;

namespace Services
{
    /// <summary>
    /// 一个作用域内的服务集合。
    /// 作用域由外部决定：Global（跨场景） 或 某个 Scene（仅该场景）。
    /// </summary>
    internal sealed class ServiceManager
    {
        private readonly Dictionary<Type, Service> serviceDict = new Dictionary<Type, Service>();

        /// <summary>是否为全局作用域</summary>
        public bool IsGlobal { get; }

        /// <summary>所属场景（Global 时为 default(Scene)）</summary>
        public UnityEngine.SceneManagement.Scene Scene { get; }

        public ServiceManager(bool isGlobal, UnityEngine.SceneManagement.Scene scene)
        {
            IsGlobal = isGlobal;
            Scene = scene;
        }

        public void Register(Service service, EConflictSolution solution)
        {
            Type type = service.RegisterType;

            Debugger.Settings.Copy();
            Debugger.Settings.SetAllowLog(EMessageType.Service, false);

            bool contain = TryGet(type, out Service oldService);

            Debugger.Settings.Paste();

            if (contain)
            {
                if (SolveConflict(oldService, service, solution))
                    serviceDict.Add(type, service);
            }
            else
                serviceDict.Add(type, service);
        }

        public void Unregister(Service service)
        {
            Type type = service.RegisterType;
            if (!TryGet(type, out Service ret))
                return;
            if (ret != service)
                return;

            serviceDict.Remove(type);
        }

        public T Get<T>() where T : class, IService
            => Get(typeof(T)) as T;

        public Service Get(Type type)
        {
            if (TryGet(type, out Service ret))
                return ret;
            throw new InvalidOperationException($"服务{type}未注册");
        }

        public bool TryGet(Type type, out Service ret)
        {
            if (!serviceDict.ContainsKey(type))
            {
                if (!IService.ExtendsIService(type))
                {
                    Type i = IService.GetSubInterfaceOfIService(type);
                    if (i != null)
                    {
                        Debugger.LogWarning($"不存在登记类型为{type}的服务,转而尝试获取登记类型为{i}的服务", EMessageType.Service);
                        return TryGet(i, out ret);
                    }
                }

                Debugger.LogError($"不存在登记类型为{type}的服务", EMessageType.Service);
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
        private bool SolveConflict(Service oldService, Service newService, EConflictSolution solution)
        {
            if (oldService == newService)
                return false;

            bool ret = false;
            Debugger.LogWarning($"服务发生冲突,旧服务{oldService.Information};\n新服务{newService.Information};解决方式为{solution}", EMessageType.Service);
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

        /// <summary>清空字典（不销毁服务 GameObject）</summary>
        public void Clear()
        {
            serviceDict.Clear();
        }

        /// <summary>场景卸载时调用：Unregister 所有服务并销毁其 GameObject</summary>
        public void DisposeAll()
        {
            // 复制一份引用，Unregister 会修改字典
            var services = new List<Service>(serviceDict.Values);
            serviceDict.Clear();
            for (int i = 0; i < services.Count; i++)
            {
                var s = services[i];
                if (s != null)
                    s.Destroy();
            }
        }
    }
}
