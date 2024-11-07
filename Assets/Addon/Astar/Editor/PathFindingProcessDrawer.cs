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
            settings.PropertyField("����", NextRect(settings));
            mover.PropertyField("�ƶ���", NextRect(mover));
            mono.PropertyField("�ű�", NextRectRelative());
            output.ListField("���·��", NextRect(output));
            available.ListField("�ɴ�λ��", NextRect(available));
            isRunning.BoolField("������", NextRectRelative());
            from.PropertyField("���", NextRect(from));
            to.PropertyField("�յ�", NextRect(to));
            currentNode.PropertyField("��ǰȷ��·���ڵ�", NextRect(currentNode));
            nearest.PropertyField("���յ�����ɴ�ڵ�", NextRect(nearest));
            countOfCloseNode.IntField("��ȷ��·���ڵ���", NextRectRelative());
            countOfQuery.IntField("��ѯ�ڵ����", NextRectRelative());
        }
    }
}