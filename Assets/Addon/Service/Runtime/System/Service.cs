using System;
using System.Reflection;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// ��ͨ��Get��ȡ�̳д�����������ÿ����Ķ���Ӧ����Ψһ��
    /// </summary>
    public class Service : MonoBehaviour
    {
        /// <summary>
        /// ע��ʱ������Type�������ű�Ҫ��ȡ�������ʱ��Ҳʹ��ע��ʱ������Type
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
    ///�Զ���ȡ�������񣬽���Service��������ʹ��
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class OtherAttribute : Attribute
    {
        internal Type type;

        /// <param name="_type">��ȡB��ʵ������ֵ��A��ʱ��B��̳�A�ࣩ����Ҫָ��B�������Ϊ������������ָ������</param>
        public OtherAttribute(Type _type = null)
        {
            type = _type;
        }
    }
}