using System;
using System.Reflection;

namespace Tools
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
    }
}