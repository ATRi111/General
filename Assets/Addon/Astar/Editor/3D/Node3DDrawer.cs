using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace AStar.ThreeD
{
    [CustomPropertyDrawer(typeof(Node3D), true)]
    public class Node3DDrawer : AutoPropertyDrawer
    {
        [AutoProperty]
        public SerializedProperty position, state;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.position.Vector3IntField("位置", NextRectRelative());
            state.EnumField<ENodeState>("节点状态", NextRectRelative());
        }
    }
}
