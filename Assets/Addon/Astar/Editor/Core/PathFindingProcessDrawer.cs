using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace AStar
{
    [CustomPropertyDrawer(typeof(PathFindingProcess), true)]
    public class PathFindingProcessDrawer : AutoPropertyDrawer
    {
        [AutoProperty]
        public SerializedProperty mountPoint, useBoundary;

        [AutoProperty]
        public SerializedProperty output, available, isRunning, from, to, currentNode, nearest, generateCount, queryCount, openCount;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            mountPoint.PropertyField("挂载点", NextRectRelative());
            useBoundary.BoolField("启用边界", NextRectRelative());
            if (Application.isPlaying)
            {
                AutoPropertyField("输出路径", output);
                AutoPropertyField("可达位置", available);
                EditorGUI.BeginDisabledGroup(true);
                isRunning.BoolField("运行中", NextRectRelative());
                AutoPropertyField("起点", from);
                AutoPropertyField("终点", to);
                AutoPropertyField("当前确定路径节点", currentNode);
                AutoPropertyField("离终点最近的可达节点", nearest);
                generateCount.IntField("生成节点次数", NextRectRelative());
                queryCount.IntField("位置查询次数", NextRectRelative());
                openCount.IntField("入堆节点个数",NextRectRelative());
                EditorGUI.EndDisabledGroup();
            }
        }
    }
}
