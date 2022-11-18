using System;
using System.Reflection;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// 可通过Get获取继承此类的子类对象，每个类的对象应当是唯一的
    /// </summary>
    public class Service : MonoBehaviour
    {
        /// <summary>
        /// 注册时声明的Type，其他脚本要获取此类服务时，也使用注册时声明的Type
        /// </summary>
        public virtual Type RegisterType => GetType();

        protected virtual void Awake()
        {
            ServiceLocator.Register(this);
        }

        protected void Start()
        {
            GetOtherService();
            Init();
            ServiceLocator.ServiceInit?.Invoke(this);
        }

        protected internal virtual void Init() { }

        internal void GetOtherService()
        {
            static T GetAttribute<T>(MemberInfo member, bool inherit = false) where T : Attribute
            {
                object[] ret = member.GetCustomAttributes(typeof(T), inherit);
                return ret.Length > 0 ? ret[0] as T : null;
            }

            FieldInfo[] infos = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo info in infos)
            {
                Type type = info.FieldType;
                OtherAttribute attribute = GetAttribute<OtherAttribute>(info, true);
                if (attribute != null && type.IsSubclassOf(typeof(Service)))
                {
                    if (attribute.type != null && attribute.type.IsSubclassOf(type))
                        type = attribute.type;
                    info.SetValue(this, ServiceLocator.Get(type));
                }
            }
        }
    }

    /// <summary>
    ///自动获取其他服务，仅在Service的子类中使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class OtherAttribute : Attribute
    {
        internal Type type;

        /// <param name="_type">获取B类实例并赋值给A类时（B类继承A类），需要指定B类的类型为参数，否则不用指定参数</param>
        public OtherAttribute(Type _type = null)
        {
            type = _type;
        }
    }
}