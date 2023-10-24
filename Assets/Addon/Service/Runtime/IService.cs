using Services;
using System;
using System.Reflection;

public interface IService
{
    /// <summary>
    /// 判断某个接口是否直接继承IService
    /// </summary>
    public static bool ExtendsIService(Type type)
    {
        if (!type.IsInterface)
            return false;
        Type[] info = type.GetInterfaces();
        for (int i = 0; i < info.Length; i++)
        {
            if (info[i] == typeof(IService))
                return true;
        }
        return false;
    }

    /// <summary>
    /// 获取一个类的直接继承IService的接口
    /// </summary>
    public static Type GetSubInterfaceOfIService(Type type)
    {
        if (ExtendsIService(type))
            return type;
        TypeInfo info = type.GetTypeInfo();
        Type[] interfaces = info.GetInterfaces();
        for (int i = 0; i < interfaces.Length; i++)
        {
            if (ExtendsIService(interfaces[i]))
            {
                return interfaces[i];
            }
        }
        Debugger.LogError($"{type}类没有实现直接继承IService的接口", EMessageType.Service);
        return null;
    }
}
