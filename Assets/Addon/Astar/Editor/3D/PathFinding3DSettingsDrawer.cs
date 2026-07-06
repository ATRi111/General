using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace AStar.ThreeD
{
    [CustomPropertyDrawer(typeof(PathFinding3DSettings))]
    public class PathFinding3DSettingsDrawer : PathFindingSettingsDrawer
    {
        [AutoProperty]
        public SerializedProperty getAdjoinedNodesSO, calculateDistanceSO, generateNodeSO;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.MyOnGUI(position, property, label);
            AutoPropertyField("获取相邻节点的方法", getAdjoinedNodesSO);
            AutoPropertyField("计算两点间距离的方法", calculateDistanceSO);
            AutoPropertyField("生成新节点的方法", generateNodeSO);
        }
    }
}
