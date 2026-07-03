using System;
using System.Reflection;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// 继承此类表示，具有服务的典型行为，包括登记、取消登记等
    /// </summary>
    public class Service : MonoBehaviour
    {
        /// <summary>
        /// <para>登记时使用的类型，在每个作用域内，具有同一登记类型的服务应当只存在一个</para>
        /// <para>默认情况下，登记类型应该是一个接口类型；某个类继承Service，又实现该接口类型，该接口类型再继承IService；
        /// <para>但是，如果是项目中临时增加的服务类，可能不希望为其定义一个接口；这种情况下，该类继承Service，直接实现IService，此字段重写为typeof([该类])</para>
        /// </summary>
        public virtual Type RegisterType => IService.GetSubInterfaceOfIService(GetType());

        /// <summary>
        /// 当前 Service 的冲突处理方法。为 null 时则沿用 <see cref="ServiceManager"/> 的 <see cref="ServiceManager.ConflictHandler"/>。
        /// </summary>
        protected virtual Action<Service, Service> ConflictHandler => null;

        public bool isGlobal = true;

        /// <summary>服务的作用域按scene划分，以scene的handle为key，其中0表示全局作用域</summary>
        public int Handle { get; private set; }

        public string Information { get; protected set; }

        protected virtual void Awake()
        {
            //禁止将服务移动到其他场景，全局服务自动确保不销毁
            if(isGlobal)
                DontDestroyOnLoad(gameObject);
            Handle = isGlobal ? 0 : gameObject.scene.handle;
            Information = $"服务类型:{RegisterType},所在游戏物体:{gameObject.name},作用域:{(isGlobal ? "Global" :  gameObject.scene.name")}";
            ServiceLocator.Register(this, ConflictHandler);
        }

        protected virtual void Start()
        {
            AutoServiceAttribute.Apply(this);
            Init();
        }

        /// <summary>执行到此函数时，必然已经获取所需服务</summary>
        protected internal virtual void Init() { }

        public virtual void OnDestroy()
        {
            ServiceLocator.Unregister(this);
        }

        public override string ToString()
        {
            return Information;
        }
    }

    /// <summary>
    ///自动获取其他服务
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class AutoServiceAttribute : Attribute
    {
        public static void Apply(object obj)
        {
            FieldInfo[] infos = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo info in infos)
            {
                Type type = info.FieldType;
                AutoServiceAttribute attribute = ServiceUtility.GetAttribute<AutoServiceAttribute>(info, true);
                if (attribute != null)
                {
                    Debugger.Settings.Copy();
                    Debugger.Settings.SetAllowLog(EMessageType.System, false);

                    info.SetValue(obj, ServiceLocator.Get(type));

                    Debugger.Settings.Paste();

                }
            }
        }

        public AutoServiceAttribute()
        {
        }
    }
}