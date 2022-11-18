using System;
using System.Reflection;
using UnityEditor;

namespace MyEditor
{
    /// <summary>
    /// 自动获取SerializedProperty
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

        /// <param name="propertyName">原字段名称，默认与SerializedProperty的名称相同</param>
        public AutoAttribute(string propertyName = null)
        {
            this.propertyName = propertyName;
        }
    }
}
