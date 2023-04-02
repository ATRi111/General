using System;
using System.Reflection;
using UnityEngine;

namespace Character
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    /// <summary>
    /// �����Զ���ȡComponent(�޷�ȷ����private�ֶλ�������Ч)
    /// </summary>
    public class AutoComponentAttribute : Attribute
    {
        public static void Apply(MonoBehaviour mono)
        {
            FieldInfo[] fieldInfos = mono.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo info in fieldInfos)
            {
                AutoComponentAttribute attribute = info.GetCustomAttribute<AutoComponentAttribute>();
                if (attribute != null)
                {
                    info.SetValue(mono, attribute.GetComponent(mono, info.FieldType));
                }
            }
            PropertyInfo[] propertyInfos = mono.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (PropertyInfo info in propertyInfos)
            {
                AutoComponentAttribute attribute = info.GetCustomAttribute<AutoComponentAttribute>();
                if (attribute != null)
                {
                    info.SetValue(mono, attribute.GetComponent(mono, info.PropertyType));
                }
            }
        }

        public EComponentPosition position;

        public AutoComponentAttribute(EComponentPosition position = EComponentPosition.SelfOrChildren)
        {
            this.position = position;
        }

        public Component GetComponent(MonoBehaviour mono, Type type)
        {
            if (!type.IsSubclassOf(typeof(Component)))
                return null;

            Component ret = null;
            switch (position)
            {
                case EComponentPosition.Self:
                    ret = mono.GetComponent(type);
                    break;
                case EComponentPosition.SelfOrParent:
                    ret = mono.GetComponentInParent(type);
                    break;
                case EComponentPosition.SelfOrChildren:
                    ret = mono.GetComponentInChildren(type);
                    break;
                case EComponentPosition.Indeterminate:
                    ret = mono.GetComponentInChildren(type);
                    if (ret != null)
                        ret = mono.GetComponentInParent(type);
                    break;
            }
            return ret;
        }
    }

    /// <summary>
    /// �������������λ��
    /// </summary>
    public enum EComponentPosition
    {
        Self,
        SelfOrParent,
        SelfOrChildren,
        Indeterminate,
    }
}