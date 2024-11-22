using System;
using System.Collections.Generic;
using System.Reflection;

namespace MyTool
{
    public static partial class GeneralTool
    {
        //空方法即只有一条return语句的方法，编译后的MSIL码Byte数组仅包含单个元素(42)
        public static bool IsEmptyMethod(this MethodInfo info)
        {
            return info.GetMethodBody().GetILAsByteArray().Length == 1;
        }

        public static bool HasAttribute<T>(this MemberInfo info, bool inherit = false) where T : Attribute
        {
            return info.GetCustomAttributes(typeof(T), inherit).Length > 0;
        }

        public static T GetAttribute<T>(this MemberInfo info, bool inherit = false) where T : Attribute
        {
            object[] ret = info.GetCustomAttributes(typeof(T), inherit);
            return ret.Length > 0 ? ret[0] as T : null;
        }

        public static bool HasNoParameter(this MethodInfo info)
        {
            ParameterInfo[] infos = info.GetParameters();
            return infos.Length == 0;
        }

        /// <summary>
        /// 获取一个类的所有子类
        /// </summary>
        public static List<Type> GetSubclasses(Type baseClass)
        {
            // 获取所有已加载的程序集
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> subclasses = new();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (type.IsSubclassOf(baseClass) && !type.IsAbstract)
                        subclasses.Add(type);
                }
            }
            return subclasses;
        }
    }
}