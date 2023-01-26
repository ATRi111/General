using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MyEditor
{
    /// <summary>
    /// �Զ���ȡSerializedProperty
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class AutoPropertyAttribute : Attribute
    {
        public static void Apply(object obj, SerializedObject serializedObject)
        {
            FieldInfo[] infos = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo info in infos)
            {
                if (TryGetPropertyName(info, out string name))
                {
                    SerializedProperty temp = serializedObject.FindProperty(name);
                    if (temp != null)
                        info.SetValue(obj, temp);
                    else
                        Debug.Log($"{serializedObject.targetObject.GetType()}�����Ҳ�����Ϊ{name}���ֶ�");
                }
            }
        }

        public static void ApplyRelative(object obj, SerializedProperty serializedProperty)
        {
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo info in fields)
            {
                if (TryGetPropertyName(info, out string name))
                {
                    SerializedProperty temp = serializedProperty.FindPropertyRelative(name);
                    if (temp != null)
                        info.SetValue(obj, serializedProperty.FindPropertyRelative(name));
                    else
                        Debug.Log($"{serializedProperty.name}�Ҳ�����Ϊ{name}���ֶ�");
                }
            }
        }

        public static bool TryGetPropertyName(FieldInfo info, out string ret)
        {
            ret = null;
            AutoPropertyAttribute attribute = MyEditorUtility.GetAttribute<AutoPropertyAttribute>(info);
            if (attribute == null)
                return false;
            if (info.FieldType != typeof(SerializedProperty))
                return false;

            ret = attribute.propertyName ?? info.Name;
            return true;
        }

        public string propertyName;

        /// <param name="propertyName">ԭ�ֶ����ƣ�Ĭ����SerializedProperty��������ͬ</param>
        public AutoPropertyAttribute(string propertyName = null)
        {
            this.propertyName = propertyName;
        }
    }
}
