using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MyEditor
{
    /// <summary>
    /// ���ڷ�Object���࣬������Ҫ�Զ���༭������
    /// ������������ΪSerializedProperty������Editor���У�Editor����ʹ�ô���
    /// �̳д���ʱ�����е�SerializedPropertyӦ����Ϊpublic
    /// </summary>
    public abstract class IndirectEditor
    {
        public bool foldout;
        public string label;

        public virtual void Initialize(SerializedProperty serializedProperty, string label)
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo info in fields)
            {
                if (AutoAttribute.TryGetPropertyName(info, out string name))
                {
                    SerializedProperty temp = serializedProperty.FindPropertyRelative(name);
                    if (temp != null)
                        info.SetValue(this, serializedProperty.FindPropertyRelative(name));
                    else
                        Debug.Log($"{serializedProperty.name}�Ҳ�����Ϊ{name}���ֶ�");
                }
            }
            foldout = false;
            this.label = label;
        }

        public virtual void OnInspectorGUI()
        {
            //FoldoutGroup����Ƕ��
            foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, label);
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (foldout)
            {
                EditorGUI.indentLevel++;
                MyOnInspectorGUI();
                EditorGUI.indentLevel--;
            }
        }

        protected abstract void MyOnInspectorGUI();
    }
}