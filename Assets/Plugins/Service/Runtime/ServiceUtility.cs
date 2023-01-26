using System;
using System.Reflection;

namespace Services
{
    public static class ServiceUtility
    {
        public static T GetAttribute<T>(MemberInfo member, bool inherit = false) where T : Attribute
        {
            object[] ret = member.GetCustomAttributes(typeof(T), inherit);
            return ret.Length > 0 ? ret[0] as T : null;
        }
    }
}