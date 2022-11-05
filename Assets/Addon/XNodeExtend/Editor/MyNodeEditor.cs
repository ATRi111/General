using System.Reflection;
using UnityEditor;
using XNodeEditor;

namespace XNodeExtend
{
    public abstract class MyNodeEditor : NodeEditor
    {
        public override void OnCreate()
        {
            base.OnCreate();
            FieldInfo[] fields = GetType().GetFields();
            foreach (FieldInfo info in fields)
            {
                if (info.FieldType == typeof(SerializedProperty))
                    info.SetValue(this, serializedObject.FindProperty(info.Name));
            }
        }

        /// <summary>
        /// 通常，不应该重写此方法，而应该重写MyOnBodyGUI
        /// </summary>
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            MyOnBodyGUI();

            serializedObject.ApplyModifiedProperties();
        }

        protected abstract void MyOnBodyGUI();
    }
}