using UnityEditor;
using UnityEngine;

namespace AStar.Sample
{
    [CustomEditor(typeof(PathFindingSample))]
    public class PathFindingSampleEditor : Editor
    {
        public SerializedProperty getAdjoinedNodesSO, process, hCostWeight, moveAbility;
        public PathFindingSample sample;

        private void OnEnable()
        {
            sample = target as PathFindingSample;
            getAdjoinedNodesSO = serializedObject.FindProperty(nameof(getAdjoinedNodesSO));
            process = serializedObject.FindProperty(nameof(process));
            hCostWeight = serializedObject.FindProperty(nameof(hCostWeight));
            moveAbility = serializedObject.FindProperty(nameof(moveAbility));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.BeginVertical();

            EditorGUILayout.PropertyField(getAdjoinedNodesSO, new GUIContent("��ȡ���ڽڵ�ķ���"));
            hCostWeight.floatValue = EditorGUILayout.FloatField("hCostȨ��", hCostWeight.floatValue);
            moveAbility.floatValue = EditorGUILayout.FloatField("�ƶ���", moveAbility.floatValue);
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
                EditorGUILayout.PropertyField(process, new GUIContent("Ѱ·����"));
            }

            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}