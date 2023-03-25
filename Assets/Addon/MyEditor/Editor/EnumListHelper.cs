using System;
using UnityEditor;
using UnityEngine.Events;

namespace MyEditor
{
    /// <summary>
    /// ���ڻ�����ö�����Ͷ�Ӧ���б�(������һ��Ĵ�0��ʼ������ö��)
    /// </summary>
    public class EnumListDrawer
    {
        /// <summary>
        /// �����б��е���Ԫ�صķ�����������Ԫ�������ţ������Ŷ�Ӧ��ö�ٳ��������ƣ�Ԫ�ض�Ӧ��SerializedProperty
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
        /// ö�ٳ�������Ŀ�ı�ʱ����Ҫ���ô˷����޸�
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