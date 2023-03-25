using System;
using System.Reflection;

namespace XNodeExtend
{
    public static class MyXNodeUtility
    {
        public static bool HasAttribute<T>(MemberInfo member, bool inherit = false) where T : Attribute
        {
            return member.GetCustomAttributes(typeof(T), inherit).Length > 0;
        }

        public static T GetAttribute<T>(MemberInfo member, bool inherit = false) where T : Attribute
        {
            object[] ret = member.GetCustomAttributes(typeof(T), inherit);
            return ret.Length > 0 ? ret[0] as T : null;
        }
    }
}