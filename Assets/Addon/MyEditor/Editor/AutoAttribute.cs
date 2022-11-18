using System;
using System.Reflection;
using UnityEditor;

namespace MyEditor
{
    /// <summary>
    /// �Զ���ȡSerializedProperty
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class AutoAttribute : Attribute
    {
        public static bool TryGetPropertyName(FieldInfo info, out string ret)
        {
            ret = null;
            AutoAttribute attribute = MyEditorUtility.GetAttribute<AutoAttribute>(info);
            if (attribute == null)
                return false;
            if (info.FieldType != typeof(SerializedProperty))
                return false;

            ret = attribute.propertyName ?? info.Name;
            return true;
        }

        public string propertyName;

        /// <param name="propertyName">ԭ�ֶ����ƣ�Ĭ����SerializedProperty��������ͬ</param>
        public AutoAttribute(string propertyName = null)
        {
            this.propertyName = propertyName;
        }
    }
}
