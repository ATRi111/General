using UnityEditor;
using UnityEngine;

namespace AStar
{
    [CustomEditor(typeof(PathFindingSample))]
    public class PathFindingSampleEditor : Editor
    {
        public SerializedProperty calculateWeightSO, calculateHCostSO, getAdjoinedNodesSO, process;
        public PathFindingSample sample;

        private void OnEnable()
        {
            sample = target as PathFindingSample;
            calculateWeightSO = serializedObject.FindProperty(nameof(calculateWeightSO));
            calculateHCostSO = serializedObject.FindProperty(nameof(calculateHCostSO));
            getAdjoinedNodesSO = serializedObject.FindProperty(nameof(getAdjoinedNodesSO));
            process = serializedObject.FindProperty(nameof(process));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.BeginVertical();

            EditorGUILayout.PropertyField(calculateWeightSO, new GUIContent("����Ȩ�صķ���"));
            EditorGUILayout.PropertyField(calculateHCostSO, new GUIContent("����HCost�ķ���"));
            EditorGUILayout.PropertyField(getAdjoinedNodesSO, new GUIContent("��ȡ���ڽڵ�ķ���"));

            if (Application.isPlaying)
            {
                if (GUILayout.Button("��ʼѰ·"))
                    sample.StartPathFinging();
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