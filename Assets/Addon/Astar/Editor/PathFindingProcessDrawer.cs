using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace AStar
{
    [CustomPropertyDrawer(typeof(PathFindingProcess),true)]
    public class PathFindingProcessDrawer : AutoPropertyDrawer
    {
        [AutoProperty]
        public SerializedProperty settings, mover, mono, output, available, isRunning, from, to, currentNode, nearest, countOfCloseNode, countOfQuery;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            AutoPropertyField("设置", settings);
            AutoPropertyField("移动者", mover);
            mono.PropertyField("脚本", NextRectRelative());
            AutoPropertyField("输出路径", output);
            AutoPropertyField("可达位置", available);
            isRunning.BoolField("运行中", NextRectRelative());
            AutoPropertyField("起点", from);
            AutoPropertyField("终点", to);
            AutoPropertyField("当前确定路径节点", currentNode);
            AutoPropertyField("离终点最近可达节点", nearest);
            countOfCloseNode.IntField("已确定路径节点数", NextRectRelative());
            countOfQuery.IntField("查询节点次数", NextRectRelative());
        }
    }
}