using System;
using System.Collections.Generic;

namespace Services
{
    /// <summary>
    /// 一个 handle 作用域内的服务集合。
    /// handle = 0 表示 Global；正整数表示 Scene.handle。
    /// 冲突处理由 <see cref="ConflictHandler"/> 委托驱动，外部可按需替换。
    /// </summary>
    internal sealed class ServiceManager
    {
        private readonly Dictionary<Type, Service> serviceDict = new Dictionary<Type, Service>();

        /// <summary>每个ServiceManager管理一个作用域(场景)，以scene.handle为标识符，handle = 0表示Global</summary>
        public int Handle { get; private set; }

        /// <summary>
        /// 同一 RegisterType 重复注册时的冲突处理方法。
        /// <para>签名：(旧服务, 新服务) → 无返回值。</para>
        /// <para>冲突时只调用本方法做善后（销毁旧组件、注销旧项等），新服务不会进入字典；
        /// 不冲突时直接注册新服务，不会调用本方法。默认实现 <see cref="DefaultConflictHandler"/>：仅记日志。</para>
        /// </summary>
        public Action<Service, Service> ConflictHandler { get; set; } = DefaultConflictHandler;

        public ServiceManager(int handle)
        {
            Handle = handle;
        }

        public void Register(Service service, Action<Service, Service> conflictHandler)
        {
            Type type = service.RegisterType;

            Debugger.Settings.Copy();
            Debugger.Settings.SetAllowLog(EMessageType.Service, false);

            bool contain = TryGet(type, out Service oldService);

            Debugger.Settings.Paste();

            // 不冲突：直接注册
            if (!contain)
            {
                serviceDict.Add(type, service);
                return;
            }

            Action<Service, Service> handler = conflictHandler ?? ConflictHandler;
            handler(oldService, service);
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
                    if (i != null && i != type)
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
        /// 默认冲突处理：新服务不会注册，也不会被销毁
        /// </summary>
        public static void DefaultConflictHandler(Service oldService, Service newService)
        {
            Debugger.LogWarning($"服务发生冲突,旧服务{oldService.Information};\n新服务{newService.Information};采用默认策略:不注册也不销毁新服务", EMessageType.Service);
        }
    }
}
