using System;
using System.Reflection;
using UnityEngine;

namespace Character
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AutoHashAttribute : Attribute
    {
        public static void Apply(MonoBehaviour mono)
        {
            FieldInfo[] fieldInfos = mono.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo info in fieldInfos)
            {
                if (info.FieldType == typeof(int))
                {
                    AutoHashAttribute attribute = info.GetCustomAttribute<AutoHashAttribute>();
                    if (attribute != null)
                    {
                        info.SetValue(mono, Animator.StringToHash(attribute.name));
                    }
                }
            }
        }

        private readonly string name;
        public AutoHashAttribute(string name)
        {
            this.name = name;
        }
    }
}