using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace Character
{
    [CustomPropertyDrawer(typeof(CharacterProperty))]
    public class CharacterPropertyDrawer : AutoPropertyDrawer
    {
        protected override bool AlwaysFoldout => true;
        [AutoProperty]
        public SerializedProperty defaultValue, currentValue;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            defaultValue.FloatField("默认值", NextRectRelative());
            if (Application.isPlaying)
            {
                EditorGUI.BeginDisabledGroup(true);
                currentValue.FloatField("当前值", NextRectRelative());
                EditorGUI.EndDisabledGroup();
            }
        }
    }
}