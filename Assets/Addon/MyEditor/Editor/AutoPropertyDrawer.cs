using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MyEditor
{
    public abstract class AutoPropertyDrawer : PropertyDrawer
    {
        public Rect[] DevideRectVertical(Rect rect,int count)
        {
            Rect[] rects = new Rect[count];
            float height = rect.height / count;
            float delta = 0;
            for (int i = 0; i < count; i++)
            {
                rects[i] = new Rect(rect.x, rect.y + delta, rect.width, height);
                delta += height;
            }
            return rects;
        }

        /// <summary>
        /// ͨ����˵����Ӧ����д�˷�������Ӧ����дMyOnGUI
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            Initialize(property);
            MyOnGUI(position,property,label);
            EditorGUI.EndProperty();
        }

        protected abstract void MyOnGUI(Rect position, SerializedProperty property, GUIContent label);

        public virtual void Initialize(SerializedProperty property)
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo info in fields)
            {
                if (AutoPropertyAttribute.TryGetPropertyName(info, out string name))
                {
                    SerializedProperty temp = property.FindPropertyRelative(name);
                    if (temp != null)
                        info.SetValue(this, property.FindPropertyRelative(name));
                    else
                        Debug.Log($"{property.name}���Ҳ�����Ϊ{name}���ֶ�");
                }
            }
        }
    }
}