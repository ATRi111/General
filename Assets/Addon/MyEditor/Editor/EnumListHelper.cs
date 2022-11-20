using System;
using UnityEditor;
using UnityEngine.Events;

namespace MyEditor
{
    /// <summary>
    /// 用于绘制与枚举类型对应的列表
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
            foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, label);
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (foldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < list.arraySize; i++)
                {
                    string enumName = Enum.ToObject(enumType,i).ToString();
                    SerializedProperty serializedProperty = list.GetArrayElementAtIndex(i);
                    DrawElement?.Invoke(i, enumName, serializedProperty);
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}