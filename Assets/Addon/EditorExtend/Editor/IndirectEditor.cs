using UnityEditor;

namespace EditorExtend
{
    /// <summary>
    /// 用于非Object子类，但是需要自定义编辑器的类
    /// 此类管理的类作为SerializedProperty出现在Editor类中，Editor类间接使用此类
    /// </summary>
    public abstract class IndirectEditor
    {
        public bool foldout;
        public string label;
        protected virtual string DefaultLabel => string.Empty;
        
        public IndirectEditor(SerializedProperty serializedProperty, string label = null)
        {
            AutoPropertyAttribute.ApplyRelative(this, serializedProperty);
            foldout = true;
            this.label = label ?? DefaultLabel;
        }

        public virtual void OnInspectorGUI()
        {
            //FoldoutHeaderGroup不能嵌套，所以这里模仿嵌套的视觉效果，但没有嵌套
            foldout = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), foldout, label);
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