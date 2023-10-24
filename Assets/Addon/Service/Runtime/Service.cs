using System;
using System.Reflection;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// �̳д����ʾ�����з���ĵ�����Ϊ�������Ǽǡ�ȡ���Ǽǵ�
    /// </summary>
    [Serializable]
    public class Service : MonoBehaviour
    {
        /// <summary>
        /// <para>�Ǽ�ʱʹ�õ����ͣ�����ͬһ�Ǽ����͵�ʵ��Ӧ��ֻ����һ��</para>
        /// <para>Ĭ������£��Ǽ�����Ӧ����һ���ӿ����ͣ�ĳ����̳�Service����ʵ�ָýӿ����ͣ��ýӿ������ټ̳�IService��
        /// �����ű�Ҫ��ȡһ������ʱ��Ҳ���岢��ȡ�ӿ����͵��ֶΣ�������������ע��</para>
        /// <para>���ǣ��������Ŀ����ʱ���ӵķ����࣬���ܲ�ϣ��Ϊ�䶨��һ���ӿڣ���������£�����̳�Service����ֱ��ʵ��IService���Ǽ�����ֱ��ʹ�ø��౾��</para>
        /// </summary>
        public virtual Type RegisterType => IService.GetSubInterfaceOfIService(GetType());
        protected virtual EConflictSolution Solution => EConflictSolution.DestroyNew;

        public string Informantion { get; protected set; }

        protected virtual void Awake()
        {
            Informantion = $"��������:{RegisterType},������Ϸ����:{gameObject.name}";
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
    ///�Զ���ȡ��������
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

                    info.SetValue(obj, ServiceLocator.Get(type));

                    Debugger.settings.Paste();

                }
            }
        }

        public AutoServiceAttribute()
        {
        }
    }
}