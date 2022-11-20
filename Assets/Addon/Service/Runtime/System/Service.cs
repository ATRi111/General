using System;
using System.Reflection;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// 全局可获取的对象，不应同时存在超过一个同类型的对象
    /// </summary>
    public class Service : MonoBehaviour
    {
        /// <summary>
        /// 注册时声明的Type，其他脚本要获取此类服务时，也要使用注册时声明的Type
        /// </summary>
        public virtual Type RegisterType => GetType();
        protected virtual EConflictSolution Solution => EConflictSolution.DestroyNew;

        public string Informantion { get; protected set; }

        protected virtual void Awake()
        {
            Informantion = $"服务类型:{GetType()},所在游戏物体:{gameObject.name}";
            ServiceLocator.Register(this, Solution);
        }

        protected void Start()
        {
            GetOtherService();
            Init();
            ServiceLocator.ServiceInit?.Invoke(this);
        }

        protected internal virtual void Init() { }

        public virtual void Destroy()
        {
            ServiceLocator.Unregister(this);
            Destroy(this);
        }

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

        public override string ToString()
        {
            return Informantion;
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