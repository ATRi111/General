using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace AStar
{
    [CustomPropertyDrawer(typeof(MoverBase))]
    public class MoverBaseDrawer : AutoPropertyDrawer
    {
        [AutoProperty]
        public SerializedProperty moveAbility;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            moveAbility.FloatField("默认移动力", NextRectRelative());
        }
    }
}