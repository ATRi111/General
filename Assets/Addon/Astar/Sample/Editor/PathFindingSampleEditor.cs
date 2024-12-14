using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace AStar.Sample
{
    [CustomEditor(typeof(PathFindingSample))]
    public class PathFindingSampleEditor : AutoEditor
    {
        [AutoProperty]
        public SerializedProperty prefab, process, moveAbility;
        public PathFindingSample sample;

        protected override void OnEnable()
        {
            base.OnEnable();
            sample = target as PathFindingSample;
        }

        protected override void MyOnInspectorGUI()
        {
            prefab.PropertyField("Ԥ����");
            moveAbility.IntField("�ƶ���");
            process.PropertyField("Ѱ·����");
            if (Application.isPlaying)
            {
                if (GUILayout.Button("��ʼѰ·"))
                    sample.StartPathFinding();
                if (GUILayout.Button("��һ��"))
                    sample.Next();
                if (GUILayout.Button("�������Ѱ·"))
                    sample.Complete();
                if (GUILayout.Button("��ս��"))
                    sample.Clear();
            }
        }
    }
}