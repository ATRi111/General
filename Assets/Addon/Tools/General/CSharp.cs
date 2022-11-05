using System;
using System.Reflection;

namespace Tools
{
    public static partial class GeneralTool
    {
        //�շ�����ֻ��һ��return���ķ�����������MSIL��Byte�������������Ԫ��(42)
        public static bool IsEmptyMethod(MethodInfo info)
        {
            return info.GetMethodBody().GetILAsByteArray().Length == 1;
        }

        public static bool HasAttribute<T>(this MemberInfo member, bool inherit = false) where T : Attribute
        {
            return member.GetCustomAttributes(typeof(T), inherit).Length > 0;
        }

        public static T GetAttribute<T>(this MemberInfo member, bool inherit = false) where T : Attribute
        {
            object[] ret = member.GetCustomAttributes(typeof(T), inherit);
            return ret.Length > 0 ? ret[0] as T : null;
        }
    }
}

