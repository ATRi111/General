using System;
using System.Reflection;
using Tools;

namespace Services.Save
{
    /// <summary>
    /// ��ҪΪ���ָ����Ķ��������浵�����࣬����Ҫ����Щ��������ӵ������С�
    /// ���ǣ���Ҫд�ڴ��ļ��У�Ӧ�úʹ浵������д��һ��
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
        /// �ô����Ա�ǵķ�������WholeSaveData��Init�����е���
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        private class InitAttribute : Attribute
        {

        }
    }
}