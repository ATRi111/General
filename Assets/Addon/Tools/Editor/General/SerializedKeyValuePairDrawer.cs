using UnityEditor;
using UnityEngine;

namespace MyTool
{
    [CustomPropertyDrawer(typeof(SerializedKeyValueBase),true)]
    public class SerializedKeyValuePairDrawer : PropertyDrawer
    {
        protected bool foldout;
        protected Vector2 min;
        protected float width;
        protected float totalHeight;

        public Rect NextRectRelative(float multiplier = 1, float gapMultiplier = 0.111f)
            => NextRect(multiplier * EditorGUIUtility.singleLineHeight, gapMultiplier * EditorGUIUtility.singleLineHeight);
        public Rect NextRect(SerializedProperty property)
            => NextRect(EditorGUI.GetPropertyHeight(property, property.isArray), 0f);
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

        public SerializedProperty key, value;

        public virtual void Initialize(Rect position, SerializedProperty property)
        {
            totalHeight = 0;
            min = position.min;
            width = position.width;
            key = property.FindPropertyRelative(nameof(key));
            value = property.FindPropertyRelative(nameof(value));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Initialize(position, property);
            EditorGUI.BeginProperty(position, label, property);
            foldout = EditorGUI.Foldout(NextRectRelative(), foldout, label);
            if (foldout)
            {
                EditorGUI.indentLevel++;
                Rect rect = NextRect(Mathf.Max(40, EditorGUI.GetPropertyHeight(property, property.isArray)) + 20);
                Vector2 h = new(rect.width / 3, 0);
                Vector2 v = new(0, 20);
                Rect leftUp = new(rect.position, h + v);
                Rect leftDown = new(rect.position + v, h + v);
                Rect right = new(rect.position + h, rect.size - h);
                EditorGUI.LabelField(leftUp, "Key");
                SerializedDictionaryHelper.AutoField(leftDown, key, GUIContent.none);
                SerializedDictionaryHelper.AutoField(right, value, GUIContent.none);
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndProperty();
        }
    }
}