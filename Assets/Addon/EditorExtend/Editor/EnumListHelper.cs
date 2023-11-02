using System;
using UnityEditor;
using UnityEngine.Events;

namespace EditorExtend
{
    /// <summary>
    /// 用于绘制与枚举类型对应的列表(必须是一般的从0开始递增的枚举)
    /// </summary>
    public class EnumListDrawer
    {
        /// <summary>
        /// 绘制列表中单个元素的方法，参数：元素索引号，索引号对应的枚举常量的名称，元素对应的SerializedProperty
        /// </summary>
        public event UnityAction<int, string, SerializedProperty> DrawElement;

        public bool foldout;
        public string label;
        public SerializedProperty list;
        public Type enumType;

        public EnumListDrawer(string label, SerializedProperty list, Type enumType)
        {
            this.label = label;
            this.list = list;
            this.enumType = enumType;
            foldout = true;
        }

        public void OnInspectorGUI()
        {
            Fix();
            foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, label);
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (foldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < list.arraySize; i++)
                {
                    string enumName = Enum.ToObject(enumType, i).ToString();
                    SerializedProperty serializedProperty = list.GetArrayElementAtIndex(i);
                    DrawElement?.Invoke(i, enumName, serializedProperty);
                }
                EditorGUI.indentLevel--;
            }
        }

        /// <summary>
        /// 枚举常量的数目改变时，需要调用此方法修复
        /// </summary>
        private void Fix()
        {
            int length = Enum.GetValues(enumType).Length;
            int current = list.arraySize;
            if (length > current)
            {
                for (int i = current; i < length; i++)
                {
                    list.InsertArrayElementAtIndex(i);
                }
            }
            else
            {
                for (int i = current - 1; i > length - 1; i--)
                {
                    list.DeleteArrayElementAtIndex(i);
                }
            }
        }
    }
}