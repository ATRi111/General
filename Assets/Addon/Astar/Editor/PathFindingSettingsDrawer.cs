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
            capacity.IntField("堆容量", NextRectRelative());
            maxDepth.IntField("最大Closed节点数", NextRectRelative());
            hCostWeight.FloatField("HCost权重权重", NextRectRelative());
        }
    }
}