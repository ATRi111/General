using System.Reflection;
using UnityEditor;

namespace MyEditor
{
    /// <summary>
    /// 用于非Object子类，但是需要自定义编辑器的类
    /// 此类管理的类作为SerializedProperty出现在Editor类中，Editor类间接使用此类
    /// 继承此类时，所有的SerializedProperty应声明为public
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
                if (MyEditorUtility.HasAttribute<AutoAttribute>(info) && info.FieldType == typeof(SerializedProperty))
                    info.SetValue(this, serializedProperty.FindPropertyRelative(info.Name));
            }
            foldout = false;
            this.label = label;
        }

        public virtual void OnInspectorGUI()
        {
            //FoldoutGroup不能嵌套
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