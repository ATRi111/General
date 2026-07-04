using EditorExtend;
using UnityEditor;
using UnityEngine;
using AStar;

namespace AStar.TwoD
{
    [CustomPropertyDrawer(typeof(Node2D), true)]
    public class AStarNodeDrawer : AutoPropertyDrawer
    {
        [AutoProperty]
        public SerializedProperty position, state;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.position.Vector2IntField("位置", NextRectRelative());
            state.EnumField<ENodeState>("节点状态", NextRectRelative());
        }
    }
}