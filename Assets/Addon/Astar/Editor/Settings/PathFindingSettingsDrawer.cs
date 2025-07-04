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
            capacity.IntField("������", NextRectRelative());
            maxDepth.IntField("���Closed�ڵ���", NextRectRelative());
            hCostWeight.FloatField("HCostȨ��", NextRectRelative());
            AutoPropertyField("��ȡ���ڽڵ�ķ���", getAdjoinedNodesSO);
            AutoPropertyField("������������ķ���", calculateDistanceSO);
            AutoPropertyField("�����½ڵ�ķ���", generateNodeSO);
        }
    }
}