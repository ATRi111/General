using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace AStar
{
    [CustomPropertyDrawer(typeof(PathFindingSettings))]
    public class PathFindingSettingsDrawer : AutoPropertyDrawer
    {
        [AutoProperty]
        public SerializedProperty capacity, maxDepth, hCostWeight;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            capacity.IntField("������", NextRectRelative());
            maxDepth.IntField("���Closed�ڵ���", NextRectRelative());
            hCostWeight.FloatField("HCostȨ��Ȩ��", NextRectRelative());
        }
    }
}