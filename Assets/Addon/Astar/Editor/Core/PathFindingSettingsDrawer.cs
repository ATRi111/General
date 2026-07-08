using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace AStar
{
    [CustomPropertyDrawer(typeof(PathFindingSettings), true)]
    public class PathFindingSettingsDrawer : AutoPropertyDrawer
    {
        protected override bool AlwaysFoldout => true;

        [AutoProperty]
        public SerializedProperty heapCapacity, cacheCapacity, temporaryCacheCapacity, hCostWeight;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            heapCapacity.IntField("堆容量", NextRectRelative());
            cacheCapacity.IntField("节点缓存容量", NextRectRelative());
            temporaryCacheCapacity.IntField("节点临时缓存容量", NextRectRelative());
            hCostWeight.FloatField("HCost权重", NextRectRelative());
        }
    }
}
