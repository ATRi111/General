using System;
using System.Reflection;
using Tools;

/// <summary>
/// 需要为各种各样的对象新增存档数据类，且需要将这些数据类添加到此类中。
/// 但是，不要写在此文件中，应该和存档数据类写在一起
/// </summary>
[Serializable]
public sealed partial class WholeSaveData
{
    public WholeSaveData()
    {
        Init();
    }

    private void Init()
    {
        MethodInfo[] infos = typeof(WholeSaveData).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (MethodInfo info in infos)
        {
            if (info.HasAttribute<InitAttribute>() && info.HasNoParameter())
            {
                info.Invoke(this, null);
            }
        }
    }

    /// <summary>
    /// 用此属性标记的方法会在WholeSaveData的Init函数中调用
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    private class InitAttribute : Attribute
    {

    }
}