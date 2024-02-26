using System;
using System.Reflection;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// 继承此类表示，具有服务的典型行为，包括登记、取消登记等
    /// </summary>
    [Serializable]
    public class Service : MonoBehaviour
    {
        /// <summary>
        /// <para>登记时使用的类型，具有同一登记类型的实例应当只存在一个</para>
        /// <para>默认情况下，登记类型应该是一个接口类型；某个类继承Service，又实现该接口类型，该接口类型再继承IService；
        /// 其他脚本要获取一个服务时，也定义并获取接口类型的字段，这体现了依赖注入</para>
        /// <para>但是，如果是项目中临时增加的服务类，可能不希望为其定义一个接口；这种情况下，该类继承Service，并直接实现IService，登记类型直接使用该类本身</para>
        /// </summary>
        public virtual Type RegisterType => IService.GetSubInterfaceOfIService(GetType());
        protected virtual EConflictSolution Solution => EConflictSolution.DestroyNew;

        public string Informantion { get; protected set; }

        protected virtual void Awake()
        {
            Informantion = $"服务类型:{RegisterType},所在游戏物体:{gameObject.name}";
            ServiceLocator.Register(this, Solution);
        }

        protected virtual void Start()
        {
            AutoServiceAttribute.Apply(this);
            Init();
            ServiceLocator.ServiceInit?.Invoke(this);
        }

        protected internal virtual void Init() { }

        public virtual void Destroy()
        {
            ServiceLocator.Unregister(this);
            Destroy(this);
        }

        public override string ToString()
        {
            return Informantion;
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