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
            AutoPropertyField("����", settings);
            AutoPropertyField("�ƶ���", mover);
            mono.PropertyField("�ű�", NextRectRelative());
            AutoPropertyField("���·��", output);
            AutoPropertyField("�ɴ�λ��", available);
            isRunning.BoolField("������", NextRectRelative());
            AutoPropertyField("���", from);
            AutoPropertyField("�յ�", to);
            AutoPropertyField("��ǰȷ��·���ڵ�", currentNode);
            AutoPropertyField("���յ�����ɴ�ڵ�", nearest);
            countOfCloseNode.IntField("��ȷ��·���ڵ���", NextRectRelative());
            countOfQuery.IntField("��ѯ�ڵ����", NextRectRelative());
        }
    }
}