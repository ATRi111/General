using System;
using System.Reflection;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// 全局可获取的对象，不应同时存在超过一个同类型的对象
    /// </summary>
    [Serializable]
    public class Service : MonoBehaviour
    {
        public Type RegisterType => IService.GetSubInterfaceOfIService(GetType());
        protected virtual EConflictSolution Solution => EConflictSolution.DestroyNew;

        public string Informantion { get; protected set; }

        protected virtual void Awake()
        {
            Informantion = $"服务类型:{RegisterType},所在游戏物体:{gameObject.name}";
            ServiceLocator.Register(this, Solution);
        }

        protected virtual void Start()
        {
            AutoServiceAttribute.Apply(this);
            Init();
            ServiceLocator.ServiceInit?.Invoke(this);
        }

        protected internal virtual void Init() { }

        public virtual void Destroy()
        {
            ServiceLocator.Unregister(this);
            Destroy(this);
        }

        public override string ToString()
        {
            return Informantion;
        }
    }

    /// <summary>
    ///自动获取其他服务
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class AutoServiceAttribute : Attribute
    {
        public static void Apply(object obj)
        {
            FieldInfo[] infos = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo info in infos)
            {
                Type type = info.FieldType;
                AutoServiceAttribute attribute = ServiceUtility.GetAttribute<AutoServiceAttribute>(info, true);
                if (attribute != null)
                {
                    Debugger.settings.Copy();
                    Debugger.settings.SetAllowLog(EMessageType.System, false);
                    Type interfaceType = IService.GetSubInterfaceOfIService(type);
                    Debugger.settings.Paste();

                    if (interfaceType != null)
                        info.SetValue(obj, ServiceLocator.Get(interfaceType));
                }
            }
        }

        public AutoServiceAttribute()
        {
        }
    }
}