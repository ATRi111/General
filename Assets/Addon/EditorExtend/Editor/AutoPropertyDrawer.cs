using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorExtend
{
    public abstract class AutoPropertyDrawer : PropertyDrawer
    {
        /// <summary>
        /// 若此方法返回true,则此属性在显示时不折叠
        /// </summary>
        protected virtual bool AlwaysFoldout => false;
        /// <summary>
        /// 若此方法返回true，则不显示Label
        /// </summary>
        public virtual bool NoLabel => false;

        protected bool foldout;
        protected Vector2 min;
        protected float width;
        protected float totalHeight;

        public Rect NextRectRelative(float multiplier = 1, float gapMultiplier = 0.111f)
            => NextRect(multiplier * EditorGUIUtility.singleLineHeight, gapMultiplier * EditorGUIUtility.singleLineHeight);
        public Rect NextRect(SerializedProperty property)
            => NextRect(EditorGUI.GetPropertyHeight(property, property.isArray));
        public Rect NextRect(float height, float gap = 2f)
        {
            Rect ret = new(min, new Vector2(width, height));
            totalHeight += height + gap;
            min.y += height + gap;
            return ret;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return totalHeight;
        }

        public void AutoPropertyField(string label, SerializedProperty property)
        {
            if (property.isArray)
                property.ListField(label, NextRect(property));
            else
                property.PropertyField(label, NextRect(property));
        }

        public virtual void Initialize(Rect position, SerializedProperty property)
        {
            totalHeight = 0;
            min = position.min;
            width = position.width;
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo info in fields)
            {
                if (AutoPropertyAttribute.TryGetPropertyName(info, out string name))
                {
                    SerializedProperty temp = property.FindPropertyRelative(name);
                    if (temp != null)
                        info.SetValue(this, property.FindPropertyRelative(name));
                    else
                        Debug.Log($"{property.name}中找不到名为{name}的字段");
                }
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Initialize(position, property);
            EditorGUI.BeginProperty(position, label, property);
            if (!NoLabel)
            {
                if (AlwaysFoldout)
                    EditorGUI.LabelField(NextRectRelative(), label);
                else
                    foldout = EditorGUI.Foldout(NextRectRelative(), foldout, label);
            }
            if (NoLabel || AlwaysFoldout || foldout)
            {
                EditorGUI.indentLevel++;
                MyOnGUI(position, property, label);
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndProperty();
        }

        /// <summary>
        /// 此方法中禁止调用Layout版本的EditorGUI,必须使用NextRect
        /// </summary>
        protected abstract void MyOnGUI(Rect position, SerializedProperty property, GUIContent label);
    }
}