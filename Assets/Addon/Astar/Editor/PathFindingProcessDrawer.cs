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
            settings.PropertyField("设置", NextRect(settings));
            mover.PropertyField("移动者", NextRect(mover));
            mono.PropertyField("脚本", NextRectRelative());
            output.ListField("输出路径", NextRect(output));
            available.ListField("可达位置", NextRect(available));
            isRunning.BoolField("运行中", NextRectRelative());
            from.PropertyField("起点", NextRect(from));
            to.PropertyField("终点", NextRect(to));
            currentNode.PropertyField("当前确定路径节点", NextRect(currentNode));
            nearest.PropertyField("离终点最近可达节点", NextRect(nearest));
            countOfCloseNode.IntField("已确定路径节点数", NextRectRelative());
            countOfQuery.IntField("查询节点次数", NextRectRelative());
        }
    }
}