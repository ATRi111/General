using Services;
using System;
using System.Reflection;

/// <summary>
/// ���з���̳еĽӿ�
/// </summary>
public interface IService
{
    /// <summary>
    /// �ж�ĳ���ӿ��Ƿ�ֱ�Ӽ̳�IService
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
    /// ��ȡһ�����ֱ�Ӽ̳�IService�Ľӿ�
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
        Debugger.LogError($"{type}��û��ʵ��ֱ�Ӽ̳�IService�Ľӿ�", EMessageType.System);
        return null;
    }
}
