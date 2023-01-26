using UnityEditor;

namespace MyEditor
{
    /// <summary>
    /// ���ڷ�Object���࣬������Ҫ�Զ���༭������
    /// ������������ΪSerializedProperty������Editor���У�Editor����ʹ�ô���
    /// </summary>
    public abstract class IndirectEditor
    {
        public bool foldout;
        public string label;

        public virtual void Initialize(SerializedProperty serializedProperty, string label)
        {
            AutoPropertyAttribute.ApplyRelative(this, serializedProperty);
            foldout = true;
            this.label = label;
        }

        public virtual void OnInspectorGUI()
        {
            //FoldoutHeaderGroup����Ƕ�ף���������ģ��Ƕ�׵��Ӿ�Ч������û��Ƕ��
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