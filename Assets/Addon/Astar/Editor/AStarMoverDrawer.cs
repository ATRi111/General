using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace AStar
{
    [CustomPropertyDrawer(typeof(AStarMover))]
    public class AStarMoverDrawer : AutoPropertyDrawer
    {
        [AutoProperty]
        public SerializedProperty moveAbility;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            moveAbility.IntField("ÒÆ¶¯Á¦", NextRectRelative());
        }
    }
}