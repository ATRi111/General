using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace AStar
{
    [CustomPropertyDrawer(typeof(PathFindingSettings))]
    public class PathFindingSettingsDrawer : AutoPropertyDrawer
    {
        protected override bool AlwaysFoldout => true;

        [AutoProperty]
        public SerializedProperty capacity, maxDepth, hCostWeight, getAdjoinedNodesSO, calculateDistanceSO, generateNodeSO;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            capacity.IntField("堆容量", NextRectRelative());
            maxDepth.IntField("最大Closed节点数", NextRectRelative());
            hCostWeight.FloatField("HCost权重", NextRectRelative());
            AutoPropertyField("获取相邻节点的方法", getAdjoinedNodesSO);
            AutoPropertyField("计算两点间距离的方法", calculateDistanceSO);   
            AutoPropertyField("生成新节点的方法", generateNodeSO);
        }
    }
}