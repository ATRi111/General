using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace AStar
{
    [CustomPropertyDrawer(typeof(Node), true)]
    public class AStarNodeDrawer : AutoPropertyDrawer
    {
        [AutoProperty]
        public SerializedProperty position, state;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.position.Vector2IntField("λ��", NextRectRelative());
            state.EnumField<ENodeState>("�ڵ�״̬", NextRectRelative());
        }
    }
}