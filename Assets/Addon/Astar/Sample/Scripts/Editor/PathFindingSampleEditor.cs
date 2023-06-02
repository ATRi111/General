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

            EditorGUILayout.PropertyField(calculateWeightSO, new GUIContent("计算权重的方法"));
            EditorGUILayout.PropertyField(calculateHCostSO, new GUIContent("计算HCost的方法"));
            EditorGUILayout.PropertyField(getAdjoinedNodesSO, new GUIContent("获取相邻节点的方法"));

            if (Application.isPlaying)
            {
                if (GUILayout.Button("开始寻路"))
                    sample.StartPathFinging();
                if (GUILayout.Button("下一步"))
                    sample.Next();
                if (GUILayout.Button("立刻完成寻路"))
                    sample.Complete();
                if (GUILayout.Button("清空结果"))
                    sample.Clear();
                EditorGUILayout.PropertyField(process, new GUIContent("寻路过程"));
            }

            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}